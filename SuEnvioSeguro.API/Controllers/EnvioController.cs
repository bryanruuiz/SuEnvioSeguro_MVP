using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuEnvioSeguro.API.Data;
using SuEnvioSeguro.API.Exceptions;
using SuEnvioSeguro.API.Models;
using SuEnvioSeguro.API.Services.Facades;
using SuEnvioSeguro.API.Services.States;
using SuEnvioSeguro.API.Shared;
using System.ComponentModel.DataAnnotations;

namespace SuEnvioSeguro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ADMIN,EMPLEADO")]
    public class EnvioController : ControllerBase
    {
        private readonly FacturacionFacade _facturacionFacade;
        private readonly AppDbContext _context;

        public EnvioController(FacturacionFacade facturacionFacade, AppDbContext context)
        {
            _facturacionFacade = facturacionFacade;
            _context = context;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearEnvio([FromBody] CrearEnvioRequest request)
        {
            var resultado = await _facturacionFacade.ProcesarNuevoEnvioAsync(new ProcesarNuevoEnvioInput
            {
                ClienteDocumento = request.ClienteDocumento,
                ClienteNombre = request.ClienteNombre,
                ClienteCorreo = request.ClienteCorreo,
                ClienteTelefono = request.ClienteTelefono,
                ClienteDireccion = request.ClienteDireccion,
                UsuarioDocumento = User.ObtenerDocumentoIdentidad(),
                UsuarioId = User.ObtenerUsuarioId(),
                Descripcion = request.Descripcion,
                MunicipioDestino = request.MunicipioDestino,
                Peso = request.Peso,
                Cantidad = request.Cantidad,
                Direccion = request.Direccion,
                EsDelicado = request.EsDelicado
            });

            return CreatedAtAction(nameof(ObtenerEnvio), new { codigoEnvio = resultado.Envio.CodigoEnvio }, new
            {
                idFactura = resultado.Factura.Id,
                codigoFactura = resultado.Factura.CodigoFactura,
                idEnvio = resultado.Envio.Id,
                codigoEnvio = resultado.Envio.CodigoEnvio,
                totalAPagar = resultado.Factura.TotalAPagar,
                estado = resultado.Envio.Estado
            });
        }

        [HttpPut("actualizar-estado/{codigoEnvio}")]
        public async Task<IActionResult> ActualizarEstado(string codigoEnvio, [FromBody] ActualizarEstadoRequest request)
        {
            await ObtenerUsuarioOperativoAsync(User.ObtenerDocumentoIdentidad());

            var envio = await _context.Envios
                .FirstOrDefaultAsync(e => e.CodigoEnvio == codigoEnvio);

            if (envio == null)
            {
                throw new ResourceNotFoundException("Envío no encontrado.");
            }

            IEstadoEnvio estado = NormalizadorTexto.NormalizarClave(request.NuevoEstado) switch
            {
                EstadosEnvio.Enviado => new EstadoEnviado(),
                EstadosEnvio.Entregado => new EstadoEntregado(),
                EstadosEnvio.Cancelado => new EstadoCancelado(),
                _ => throw new BusinessRuleException("Estado inválido. Solo se permite ENVIADO, ENTREGADO o CANCELADO.")
            };

            estado.ProcesarPaquete(envio);
            _context.Envios.Update(envio);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Estado actualizado exitosamente",
                idEnvio = envio.Id,
                codigoEnvio = envio.CodigoEnvio,
                nuevoEstado = envio.Estado
            });
        }

        [HttpGet]
        public async Task<IActionResult> BuscarEnvios([FromQuery] string? estado, [FromQuery] string? municipio, [FromQuery] string? codigo)
        {
            var consulta = _context.Envios
                .Include(e => e.Factura)
                .ThenInclude(f => f.Cliente)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(estado))
            {
                var estadoNormalizado = NormalizadorTexto.NormalizarClave(estado);
                consulta = consulta.Where(e => e.Estado == estadoNormalizado);
            }

            if (!string.IsNullOrWhiteSpace(municipio))
            {
                var municipioNormalizado = NormalizadorTexto.NormalizarClave(municipio);
                consulta = consulta.Where(e => e.MunicipioDestino.ToUpper().Contains(municipioNormalizado));
            }

            if (!string.IsNullOrWhiteSpace(codigo))
            {
                consulta = consulta.Where(e => e.CodigoEnvio.Contains(codigo) || e.CodigoFactura.Contains(codigo));
            }

            var envios = await consulta
                .OrderByDescending(e => e.Id)
                .Take(100)
                .Select(e => new
                {
                    idEnvio = e.Id,
                    e.CodigoEnvio,
                    e.CodigoFactura,
                    cliente = e.Factura.Cliente.Nombre,
                    e.MunicipioDestino,
                    e.DireccionEnvio,
                    e.Peso,
                    e.Cantidad,
                    e.EsDelicado,
                    e.ValorNetoEnvio,
                    e.Estado,
                    e.Factura.Fecha,
                    e.Factura.TotalAPagar
                })
                .ToListAsync();

            return Ok(envios);
        }

        [HttpGet("{codigoEnvio}")]
        public async Task<IActionResult> ObtenerEnvio(string codigoEnvio)
        {
            var envio = await _context.Envios
                .Include(e => e.Factura)
                .FirstOrDefaultAsync(e => e.CodigoEnvio == codigoEnvio);

            if (envio == null)
            {
                throw new ResourceNotFoundException("Envío no encontrado.");
            }

            return Ok(new
            {
                idEnvio = envio.Id,
                codigoEnvio = envio.CodigoEnvio,
                codigoFactura = envio.CodigoFactura,
                idFactura = envio.FacturaId,
                descripcion = envio.DescripcionContenido,
                municipio = envio.MunicipioDestino,
                peso = envio.Peso,
                cantidad = envio.Cantidad,
                esDelicado = envio.EsDelicado,
                valor = envio.ValorNetoEnvio,
                estado = envio.Estado
            });
        }

        private async Task<Usuario> ObtenerUsuarioOperativoAsync(string usuarioDocumento)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.DocumentoIdentidad == usuarioDocumento);

            if (usuario is null)
            {
                throw new ResourceNotFoundException($"No existe un usuario con documento '{usuarioDocumento}'.");
            }

            if (!usuario.Activo)
            {
                throw new ForbiddenOperationException("El usuario está inactivo y no puede operar envíos.");
            }

            if (!RolesSistema.EsOperativo(usuario.Rol))
            {
                throw new ForbiddenOperationException("Solo un ADMIN o EMPLEADO puede operar envíos.");
            }

            return usuario;
        }
    }

    public class CrearEnvioRequest
    {
        [Required]
        public string ClienteDocumento { get; set; } = string.Empty;

        public string ClienteNombre { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo del cliente no es válido")]
        public string ClienteCorreo { get; set; } = string.Empty;

        public string ClienteTelefono { get; set; } = string.Empty;

        public string ClienteDireccion { get; set; } = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public string MunicipioDestino { get; set; } = string.Empty;

        [Required]
        [Range(1, 30, ErrorMessage = "El peso debe estar entre 1kg y 30kg")]
        public float Peso { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Required]
        public string Direccion { get; set; } = string.Empty;

        public bool EsDelicado { get; set; } = false;
    }

    public class ActualizarEstadoRequest
    {
        [Required]
        public string NuevoEstado { get; set; } = string.Empty;
    }
}

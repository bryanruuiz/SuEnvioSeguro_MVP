using SuEnvioSeguro.API.Models;
using SuEnvioSeguro.API.Data;
using Microsoft.EntityFrameworkCore;
using SuEnvioSeguro.API.Services.Factories;
using SuEnvioSeguro.API.Services.Strategies;
using SuEnvioSeguro.API.Exceptions;
using SuEnvioSeguro.API.Services.Singletons;
using SuEnvioSeguro.API.Shared;

namespace SuEnvioSeguro.API.Services.Facades
{
    public class FacturacionFacade
    {
        private readonly CalculadoraTarifas _calculadora;
        private readonly EnvioFactory _factory;
        private readonly AppDbContext _context;
        private readonly GeneradorCodigoFactura _generadorCodigoFactura;

        public FacturacionFacade(
            CalculadoraTarifas calculadora,
            EnvioFactory factory,
            AppDbContext context,
            GeneradorCodigoFactura generadorCodigoFactura)
        {
            _calculadora = calculadora;
            _factory = factory;
            _context = context;
            _generadorCodigoFactura = generadorCodigoFactura;
        }

        public async Task<(Factura Factura, Envio Envio)> ProcesarNuevoEnvioAsync(ProcesarNuevoEnvioInput input)
        {
            var municipio = await ObtenerMunicipioActivoAsync(input.MunicipioDestino);
            var usuario = await ObtenerUsuarioOperativoAsync(input.UsuarioDocumento);
            var cliente = await ObtenerOCrearClienteAsync(input);
            var ultimoIdFactura = await _context.Facturas
                .Select(f => (int?)f.Id)
                .MaxAsync() ?? 0;

            _generadorCodigoFactura.SincronizarConsecutivo(1000 + ultimoIdFactura);

            var envio = _factory.CrearEnvio(
                input.Descripcion,
                municipio.Nombre,
                input.Peso,
                input.Cantidad,
                input.Direccion,
                input.EsDelicado);

            envio.ValorNetoEnvio += _calculadora.CalcularTarifaMunicipio(municipio.Nombre);
            envio.ValorNetoEnvio += _calculadora.CalcularRecargoPeso(input.Peso);

            var factura = new Factura
            {
                CodigoFactura = _generadorCodigoFactura.GenerarSiguienteCodigo(),
                Fecha = DateTime.Now,
                Cliente = cliente,
                ClienteId = cliente.Id,
                ClienteDocumento = cliente.DocumentoIdentidad,
                Usuario = usuario,
                UsuarioId = usuario.Id,
                UsuarioDocumento = usuario.DocumentoIdentidad,
                ValorNeto = envio.ValorNetoEnvio,
                PorcentajeIVA = 0.19f,
                TotalAPagar = (float)Math.Round((decimal)envio.ValorNetoEnvio * 1.19m, 2, MidpointRounding.AwayFromZero)
            };

            envio.CodigoFactura = factura.CodigoFactura;
            envio.Factura = factura;

            _context.Facturas.Add(factura);
            _context.Envios.Add(envio);
            await _context.SaveChangesAsync();

            return (factura, envio);
        }

        private async Task<Municipio> ObtenerMunicipioActivoAsync(string municipioDestino)
        {
            var municipios = await _context.Municipios
                .Where(m => m.Activo)
                .ToListAsync();

            var municipio = municipios.FirstOrDefault(m =>
                NormalizadorTexto.NormalizarClave(m.Nombre) == NormalizadorTexto.NormalizarClave(municipioDestino));

            if (municipio is null)
            {
                throw new ResourceNotFoundException($"Municipio '{municipioDestino}' no encontrado o inactivo.");
            }

            return municipio;
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
                throw new ForbiddenOperationException("El usuario que intenta registrar el envío está inactivo.");
            }

            if (!RolesSistema.EsOperativo(usuario.Rol))
            {
                throw new ForbiddenOperationException("Solo un ADMIN o EMPLEADO puede registrar envíos.");
            }

            return usuario;
        }

        private async Task<Cliente> ObtenerOCrearClienteAsync(ProcesarNuevoEnvioInput input)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.DocumentoIdentidad == input.ClienteDocumento);

            if (cliente is null)
            {
                if (string.IsNullOrWhiteSpace(input.ClienteNombre) ||
                    string.IsNullOrWhiteSpace(input.ClienteCorreo) ||
                    string.IsNullOrWhiteSpace(input.ClienteTelefono) ||
                    string.IsNullOrWhiteSpace(input.ClienteDireccion))
                {
                    throw new BusinessRuleException("Si el cliente no existe, debe enviar nombre, correo, teléfono y dirección para registrarlo.");
                }

                cliente = new Cliente
                {
                    DocumentoIdentidad = input.ClienteDocumento,
                    Nombre = input.ClienteNombre,
                    Correo = input.ClienteCorreo,
                    Telefono = input.ClienteTelefono,
                    Direccion = input.ClienteDireccion,
                    TipoCliente = "OCASIONAL"
                };

                _context.Clientes.Add(cliente);
                return cliente;
            }

            if (!string.IsNullOrWhiteSpace(input.ClienteNombre))
            {
                cliente.Nombre = input.ClienteNombre;
            }

            if (!string.IsNullOrWhiteSpace(input.ClienteCorreo))
            {
                cliente.Correo = input.ClienteCorreo;
            }

            if (!string.IsNullOrWhiteSpace(input.ClienteTelefono))
            {
                cliente.Telefono = input.ClienteTelefono;
            }

            if (!string.IsNullOrWhiteSpace(input.ClienteDireccion))
            {
                cliente.Direccion = input.ClienteDireccion;
            }

            return cliente;
        }
    }
}

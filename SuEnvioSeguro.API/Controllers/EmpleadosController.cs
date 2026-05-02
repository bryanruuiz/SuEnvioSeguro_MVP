using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuEnvioSeguro.API.Data;
using SuEnvioSeguro.API.Exceptions;
using SuEnvioSeguro.API.Models;
using SuEnvioSeguro.API.Services;
using SuEnvioSeguro.API.Shared;
using System.ComponentModel.DataAnnotations;

namespace SuEnvioSeguro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RolesSistema.Admin)]
    public class EmpleadosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ServicioCriptografia _criptografia;

        public EmpleadosController(AppDbContext context, ServicioCriptografia criptografia)
        {
            _context = context;
            _criptografia = criptografia;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] bool incluirInactivos = true)
        {
            var consulta = _context.Usuarios.AsQueryable();

            if (!incluirInactivos)
            {
                consulta = consulta.Where(u => u.Activo);
            }

            var usuarios = await consulta
                .OrderBy(u => u.Nombre)
                .Select(u => new
                {
                    id = u.Id,
                    u.DocumentoIdentidad,
                    u.NombreUsuario,
                    u.Nombre,
                    u.Correo,
                    u.Telefono,
                    u.Direccion,
                    u.Rol,
                    u.Activo
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var usuario = await ObtenerUsuarioAsync(id);
            return Ok(ProyectarEmpleado(usuario));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearEmpleadoRequest request)
        {
            var rol = ValidarRol(request.Rol);
            await ValidarDocumentoDisponibleAsync(request.DocumentoIdentidad);
            await ValidarNombreUsuarioDisponibleAsync(request.NombreUsuario);

            var usuario = new Usuario
            {
                DocumentoIdentidad = request.DocumentoIdentidad.Trim(),
                NombreUsuario = request.NombreUsuario.Trim(),
                Nombre = request.Nombre.Trim(),
                Correo = request.Correo.Trim(),
                Telefono = request.Telefono.Trim(),
                Direccion = request.Direccion.Trim(),
                Rol = rol,
                Activo = true,
                Contrasena = _criptografia.HashearContrasena(request.Contrasena)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Created($"/api/empleados/{usuario.Id}", ProyectarEmpleado(usuario));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEmpleadoRequest request)
        {
            var usuario = await ObtenerUsuarioAsync(id);
            var rol = ValidarRol(request.Rol);

            if (!string.Equals(usuario.DocumentoIdentidad, request.DocumentoIdentidad.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                await ValidarDocumentoDisponibleAsync(request.DocumentoIdentidad, id);
            }

            if (!string.Equals(usuario.NombreUsuario, request.NombreUsuario.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                await ValidarNombreUsuarioDisponibleAsync(request.NombreUsuario, id);
            }

            if (usuario.Activo && !request.Activo)
            {
                await ValidarNoDesactivarUltimoAdminAsync(usuario.Id);
            }

            if (RolesSistema.EsAdmin(usuario.Rol) && !RolesSistema.EsAdmin(rol))
            {
                await ValidarNoDesactivarUltimoAdminAsync(usuario.Id);
            }

            usuario.DocumentoIdentidad = request.DocumentoIdentidad.Trim();
            usuario.NombreUsuario = request.NombreUsuario.Trim();
            usuario.Nombre = request.Nombre.Trim();
            usuario.Correo = request.Correo.Trim();
            usuario.Telefono = request.Telefono.Trim();
            usuario.Direccion = request.Direccion.Trim();
            usuario.Rol = rol;
            usuario.Activo = request.Activo;

            if (!string.IsNullOrWhiteSpace(request.Contrasena))
            {
                usuario.Contrasena = _criptografia.HashearContrasena(request.Contrasena);
            }

            await _context.SaveChangesAsync();
            return Ok(ProyectarEmpleado(usuario));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var usuario = await ObtenerUsuarioAsync(id);
            await ValidarNoDesactivarUltimoAdminAsync(usuario.Id);

            usuario.Activo = false;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Empleado desactivado exitosamente",
                id = usuario.Id,
                activo = usuario.Activo
            });
        }

        [HttpPatch("{id:int}/activar")]
        public async Task<IActionResult> Activar(int id)
        {
            var usuario = await ObtenerUsuarioAsync(id);
            usuario.Activo = true;
            await _context.SaveChangesAsync();
            return Ok(ProyectarEmpleado(usuario));
        }

        private async Task<Usuario> ObtenerUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario is null)
            {
                throw new ResourceNotFoundException("Empleado no encontrado.");
            }

            return usuario;
        }

        private static string ValidarRol(string rol)
        {
            var rolNormalizado = RolesSistema.Normalizar(rol);

            if (!RolesSistema.EsValido(rolNormalizado))
            {
                throw new BusinessRuleException("El rol del empleado debe ser ADMIN o EMPLEADO.");
            }

            return rolNormalizado;
        }

        private async Task ValidarDocumentoDisponibleAsync(string documentoIdentidad, int? empleadoId = null)
        {
            var documento = documentoIdentidad.Trim();
            var existe = await _context.Personas.AnyAsync(p =>
                p.DocumentoIdentidad == documento && (!empleadoId.HasValue || p.Id != empleadoId.Value));

            if (existe)
            {
                throw new BusinessRuleException("Ya existe una persona con ese documento de identidad.");
            }
        }

        private async Task ValidarNombreUsuarioDisponibleAsync(string nombreUsuario, int? empleadoId = null)
        {
            var usuarioLogin = nombreUsuario.Trim();
            var existe = await _context.Usuarios.AnyAsync(u =>
                u.NombreUsuario == usuarioLogin && (!empleadoId.HasValue || u.Id != empleadoId.Value));

            if (existe)
            {
                throw new BusinessRuleException("El nombre de usuario ya está en uso.");
            }
        }

        private async Task ValidarNoDesactivarUltimoAdminAsync(int usuarioId)
        {
            var adminsActivos = await _context.Usuarios
                .CountAsync(u => u.Activo && u.Rol == RolesSistema.Admin && u.Id != usuarioId);

            if (adminsActivos == 0)
            {
                throw new BusinessRuleException("No se puede desactivar o degradar el último administrador activo.");
            }
        }

        private static object ProyectarEmpleado(Usuario usuario)
        {
            return new
            {
                id = usuario.Id,
                usuario.DocumentoIdentidad,
                usuario.NombreUsuario,
                usuario.Nombre,
                usuario.Correo,
                usuario.Telefono,
                usuario.Direccion,
                usuario.Rol,
                usuario.Activo
            };
        }
    }

    public class CrearEmpleadoRequest
    {
        [Required]
        public string DocumentoIdentidad { get; set; } = string.Empty;

        [Required]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Contrasena { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = string.Empty;
    }

    public class ActualizarEmpleadoRequest
    {
        [Required]
        public string DocumentoIdentidad { get; set; } = string.Empty;

        [Required]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public string? Contrasena { get; set; }

        [Required]
        public string Rol { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
    }
}
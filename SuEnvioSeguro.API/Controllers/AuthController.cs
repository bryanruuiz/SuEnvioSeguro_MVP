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
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ServicioCriptografia _criptografia;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(AppDbContext context, ServicioCriptografia criptografia, JwtTokenService jwtTokenService)
        {
            _context = context;
            _criptografia = criptografia;
            _jwtTokenService = jwtTokenService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistroRequest request)
        {
            var rolSolicitado = RolesSistema.Normalizar(request.Rol);

            if (!RolesSistema.EsValido(rolSolicitado))
            {
                throw new BusinessRuleException("El rol del usuario debe ser ADMIN o EMPLEADO.");
            }

            await ValidarPermisoRegistroAsync(rolSolicitado);

            var usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.DocumentoIdentidad == request.DocumentoIdentidad);

            if (usuarioExistente != null)
            {
                throw new BusinessRuleException("Ya existe un usuario con ese documento de identidad.");
            }

            var nombreUsuario = request.NombreUsuario.Trim();
            var usuarioLoginExistente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

            if (usuarioLoginExistente != null)
            {
                throw new BusinessRuleException("El nombre de usuario ya está en uso.");
            }

            var usuario = new Usuario
            {
                DocumentoIdentidad = request.DocumentoIdentidad,
                Nombre = request.Nombre,
                Correo = request.Correo,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                NombreUsuario = nombreUsuario,
                Contrasena = _criptografia.HashearContrasena(request.Contrasena),
                Rol = rolSolicitado,
                Activo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Created($"/api/auth/usuarios/{usuario.Id}", new
            {
                message = "Usuario registrado exitosamente",
                id = usuario.Id,
                documentoIdentidad = usuario.DocumentoIdentidad,
                nombreUsuario = usuario.NombreUsuario,
                nombre = usuario.Nombre,
                rol = usuario.Rol,
                activo = usuario.Activo
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == request.NombreUsuario);

            if (usuario == null)
            {
                throw new AuthenticationApiException("Credenciales inválidas.");
            }

            if (!_criptografia.VerificarContrasena(usuario.Contrasena, request.Contrasena))
            {
                throw new AuthenticationApiException("Credenciales inválidas.");
            }

            if (!usuario.Activo)
            {
                throw new ForbiddenOperationException("El usuario está inactivo y no puede iniciar sesión.");
            }

            return Ok(new
            {
                message = "Login exitoso",
                token = _jwtTokenService.GenerarToken(usuario),
                id = usuario.Id,
                nombreUsuario = usuario.NombreUsuario,
                documentoIdentidad = usuario.DocumentoIdentidad,
                nombre = usuario.Nombre,
                rol = usuario.Rol,
                activo = usuario.Activo
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var usuarioId = User.ObtenerUsuarioId();
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario is null || !usuario.Activo)
            {
                throw new AuthenticationApiException("El usuario autenticado no existe o está inactivo.");
            }

            return Ok(new
            {
                id = usuario.Id,
                nombreUsuario = usuario.NombreUsuario,
                documentoIdentidad = usuario.DocumentoIdentidad,
                nombre = usuario.Nombre,
                correo = usuario.Correo,
                telefono = usuario.Telefono,
                direccion = usuario.Direccion,
                rol = usuario.Rol,
                activo = usuario.Activo
            });
        }

        private async Task ValidarPermisoRegistroAsync(string rolSolicitado)
        {
            if (User.Identity?.IsAuthenticated != true || !User.IsInRole(RolesSistema.Admin))
            {
                throw new ForbiddenOperationException("El registro de usuarios está reservado para administradores activos.");
            }
        }
    }

    public class RegistroRequest
    {
        [Required(ErrorMessage = "El documento de identidad es requerido")]
        public string DocumentoIdentidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        public string Correo { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Contrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es requerido")]
        public string Rol { get; set; } = string.Empty;

    }

    public class LoginRequest
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Contrasena { get; set; } = string.Empty;
    }
}

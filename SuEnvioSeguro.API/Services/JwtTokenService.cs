using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SuEnvioSeguro.API.Models;

namespace SuEnvioSeguro.API.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(Usuario usuario)
        {
            var clave = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("La clave JWT no está configurada.");
            var issuer = _configuration["Jwt:Issuer"] ?? "SuEnvioSeguro.API";
            var audience = _configuration["Jwt:Audience"] ?? "SuEnvioSeguro.Web";
            var minutosExpiracion = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var minutos)
                ? minutos
                : 240;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Name, usuario.NombreUsuario),
                new(ClaimTypes.Role, usuario.Rol),
                new("documentoIdentidad", usuario.DocumentoIdentidad),
                new("nombre", usuario.Nombre),
                new("usuarioId", usuario.Id.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutosExpiracion),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
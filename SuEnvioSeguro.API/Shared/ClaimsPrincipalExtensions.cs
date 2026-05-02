using System.Security.Claims;
using SuEnvioSeguro.API.Exceptions;

namespace SuEnvioSeguro.API.Shared
{
    public static class ClaimsPrincipalExtensions
    {
        public static string ObtenerDocumentoIdentidad(this ClaimsPrincipal user)
        {
            var documento = user.FindFirstValue("documentoIdentidad");

            if (string.IsNullOrWhiteSpace(documento))
            {
                throw new AuthenticationApiException("El token no contiene el documento de identidad del usuario.");
            }

            return documento;
        }

        public static int ObtenerUsuarioId(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(id, out var usuarioId))
            {
                throw new AuthenticationApiException("El token no contiene un identificador de usuario válido.");
            }

            return usuarioId;
        }
    }
}
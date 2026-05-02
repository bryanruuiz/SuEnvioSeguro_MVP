namespace SuEnvioSeguro.API.Exceptions
{
    public abstract class ApiException : Exception
    {
        protected ApiException(int statusCode, string title, string message) : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }

        public int StatusCode { get; }

        public string Title { get; }
    }

    public sealed class BusinessRuleException : ApiException
    {
        public BusinessRuleException(string message) : base(400, "Regla de negocio inválida", message)
        {
        }
    }

    public sealed class ResourceNotFoundException : ApiException
    {
        public ResourceNotFoundException(string message) : base(404, "Recurso no encontrado", message)
        {
        }
    }

    public sealed class ForbiddenOperationException : ApiException
    {
        public ForbiddenOperationException(string message) : base(403, "Operación no permitida", message)
        {
        }
    }

    public sealed class AuthenticationApiException : ApiException
    {
        public AuthenticationApiException(string message) : base(401, "Autenticación inválida", message)
        {
        }
    }
}
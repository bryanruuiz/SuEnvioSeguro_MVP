namespace SuEnvioSeguro.API.Shared
{
    public static class RolesSistema
    {
        public const string Admin = "ADMIN";
        public const string Empleado = "EMPLEADO";

        public static string Normalizar(string? rol) => rol?.Trim().ToUpperInvariant() ?? string.Empty;

        public static bool EsAdmin(string? rol) => Normalizar(rol) == Admin;

        public static bool EsOperativo(string? rol)
        {
            var rolNormalizado = Normalizar(rol);
            return rolNormalizado == Admin || rolNormalizado == Empleado;
        }

        public static bool EsValido(string? rol) => EsOperativo(rol);
    }

    public static class EstadosEnvio
    {
        public const string Registrado = "REGISTRADO";
        public const string Enviado = "ENVIADO";
        public const string Cancelado = "CANCELADO";
        public const string Entregado = "ENTREGADO";
    }
}
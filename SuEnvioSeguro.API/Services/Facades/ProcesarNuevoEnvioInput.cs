namespace SuEnvioSeguro.API.Services.Facades
{
    public class ProcesarNuevoEnvioInput
    {
        public string ClienteDocumento { get; set; } = string.Empty;

        public string ClienteNombre { get; set; } = string.Empty;

        public string ClienteCorreo { get; set; } = string.Empty;

        public string ClienteTelefono { get; set; } = string.Empty;

        public string ClienteDireccion { get; set; } = string.Empty;

        public string UsuarioDocumento { get; set; } = string.Empty;

        public int UsuarioId { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public string MunicipioDestino { get; set; } = string.Empty;

        public float Peso { get; set; }

        public int Cantidad { get; set; }

        public string Direccion { get; set; } = string.Empty;

        public bool EsDelicado { get; set; }
    }
}
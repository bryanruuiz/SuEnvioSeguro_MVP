using SuEnvioSeguro.API.Models;
using SuEnvioSeguro.API.Shared;

namespace SuEnvioSeguro.API.Services.Factories
{
    public class EnvioFactory
    {
        public Envio CrearEnvio(string descripcion, string municipioDestino, float peso, int cantidad, string direccion, bool esDelicado)
        {
            var envio = new Envio
            {
                CodigoEnvio = $"ENV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}",
                DescripcionContenido = descripcion,
                MunicipioDestino = municipioDestino,
                Peso = peso,
                Cantidad = cantidad,
                DireccionEnvio = direccion,
                EsDelicado = esDelicado,
                Estado = EstadosEnvio.Registrado,
                ValorNetoEnvio = esDelicado ? 5000f : 0f
            };

            return envio;
        }
    }
}

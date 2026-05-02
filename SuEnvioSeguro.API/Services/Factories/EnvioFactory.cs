using SuEnvioSeguro.API.Models;

namespace SuEnvioSeguro.API.Services.Factories
{
    public class EnvioFactory 
    {
        public Envio CrearEnvio(bool esDelicado) 
        {
            var envio = new Envio();
            envio.EsDelicado = esDelicado;
            
            if (esDelicado) {
                envio.ValorNetoEnvio += 5000f; // Aplica recargo base[cite: 1]
            }
            
            return envio;
        }
    }
}
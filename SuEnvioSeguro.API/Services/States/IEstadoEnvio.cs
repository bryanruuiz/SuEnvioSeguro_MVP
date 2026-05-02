using SuEnvioSeguro.API.Models;

namespace SuEnvioSeguro.API.Services.States
{
    public interface IEstadoEnvio 
    {
        void ProcesarPaquete(Envio envio);
    }
}
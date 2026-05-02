using SuEnvioSeguro.API.Models;

namespace SuEnvioSeguro.API.Services.States
{
    public class EstadoRegistrado : IEstadoEnvio 
    {
        public void ProcesarPaquete(Envio envio) 
        {
            envio.Estado = "REGISTRADO";
            // Lógica adicional cuando se registra
        }
    }
}
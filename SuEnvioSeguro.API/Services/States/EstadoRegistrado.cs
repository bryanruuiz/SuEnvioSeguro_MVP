using SuEnvioSeguro.API.Models;
using SuEnvioSeguro.API.Exceptions;
using SuEnvioSeguro.API.Shared;

namespace SuEnvioSeguro.API.Services.States
{
    public class EstadoRegistrado : IEstadoEnvio 
    {
        public void ProcesarPaquete(Envio envio) 
        {
            if (!string.IsNullOrWhiteSpace(envio.Estado) && envio.Estado != EstadosEnvio.Registrado)
            {
                throw new BusinessRuleException("Un envío ya procesado no puede volver al estado REGISTRADO.");
            }

            envio.Estado = EstadosEnvio.Registrado;
        }
    }
}
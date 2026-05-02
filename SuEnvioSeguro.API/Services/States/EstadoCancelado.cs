using SuEnvioSeguro.API.Models;
using SuEnvioSeguro.API.Exceptions;
using SuEnvioSeguro.API.Shared;

namespace SuEnvioSeguro.API.Services.States
{
    public class EstadoCancelado : IEstadoEnvio
    {
        public void ProcesarPaquete(Envio envio)
        {
            if (envio.Estado == EstadosEnvio.Entregado)
            {
                throw new BusinessRuleException("Un envío ENTREGADO no puede ser cancelado.");
            }

            if (envio.Estado == EstadosEnvio.Cancelado)
            {
                throw new BusinessRuleException("El envío ya se encuentra CANCELADO.");
            }

            envio.Estado = EstadosEnvio.Cancelado;
        }
    }
}

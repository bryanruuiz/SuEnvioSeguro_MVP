using SuEnvioSeguro.API.Models;
using SuEnvioSeguro.API.Exceptions;
using SuEnvioSeguro.API.Shared;

namespace SuEnvioSeguro.API.Services.States
{
    public class EstadoEntregado : IEstadoEnvio
    {
        public void ProcesarPaquete(Envio envio)
        {
            if (envio.Estado != EstadosEnvio.Enviado)
            {
                throw new BusinessRuleException("Solo los envíos ENVIADOS pueden pasar a ENTREGADO.");
            }

            envio.Estado = EstadosEnvio.Entregado;
        }
    }
}

using SuEnvioSeguro.API.Models;
using SuEnvioSeguro.API.Services.Factories;
using SuEnvioSeguro.API.Services.Strategies;
using SuEnvioSeguro.API.Services.Singletons;

namespace SuEnvioSeguro.API.Services.Facades
{
    public class FacturacionFacade 
    {
        private readonly CalculadoraTarifas _calculadora;
        private readonly EnvioFactory _factory;

        public FacturacionFacade() 
        {
            _calculadora = new CalculadoraTarifas();
            _factory = new EnvioFactory();
        }

        public Factura ProcesarNuevoEnvio() 
        {
            var factura = new Factura();
            factura.CodigoFactura = GeneradorCodigoFactura.ObtenerInstancia().GenerarSiguienteCodigo();
            
            return factura;
        }
    }
}
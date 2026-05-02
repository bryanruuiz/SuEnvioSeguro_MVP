namespace SuEnvioSeguro.API.Services.Strategies
{
    public class CalculadoraTarifas 
    {
        public float EjecutarCalculo(ITarifaMunicipioStrategy estrategia) 
        {
            return estrategia.CalcularTarifaBase();
        }
    }
}
namespace SuEnvioSeguro.API.Services.Strategies
{
    public class EstrategiaTarifaMedellin : ITarifaMunicipioStrategy
    {
        public float CalcularTarifaBase() => 5000f;
    }
}
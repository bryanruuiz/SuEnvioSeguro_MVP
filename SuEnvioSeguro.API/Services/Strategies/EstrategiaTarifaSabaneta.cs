namespace SuEnvioSeguro.API.Services.Strategies
{
    public class EstrategiaTarifaSabaneta : ITarifaMunicipioStrategy
    {
        public string Municipio => "Sabaneta";

        public float CalcularTarifaBase() => 6500f;
    }
}

namespace SuEnvioSeguro.API.Services.Strategies
{
    public class EstrategiaTarifaBello : ITarifaMunicipioStrategy
    {
        public string Municipio => "Bello";

        public float CalcularTarifaBase() => 6500f;
    }
}

namespace SuEnvioSeguro.API.Services.Strategies
{
    public class EstrategiaTarifaGirardota : ITarifaMunicipioStrategy
    {
        public string Municipio => "Girardota";

        public float CalcularTarifaBase() => 8000f;
    }
}

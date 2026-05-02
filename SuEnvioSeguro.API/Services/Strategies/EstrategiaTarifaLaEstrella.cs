namespace SuEnvioSeguro.API.Services.Strategies
{
    public class EstrategiaTarifaLaEstrella : ITarifaMunicipioStrategy
    {
        public string Municipio => "La Estrella";

        public float CalcularTarifaBase() => 6500f;
    }
}

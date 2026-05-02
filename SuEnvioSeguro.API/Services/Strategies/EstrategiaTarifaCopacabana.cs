namespace SuEnvioSeguro.API.Services.Strategies
{
    public class EstrategiaTarifaCopacabana : ITarifaMunicipioStrategy
    {
        public string Municipio => "Copacabana";

        public float CalcularTarifaBase() => 7500f;
    }
}

namespace SuEnvioSeguro.API.Services.Strategies
{
    public class EstrategiaTarifaCaldas : ITarifaMunicipioStrategy
    {
        public string Municipio => "Caldas";

        public float CalcularTarifaBase() => 7500f;
    }
}

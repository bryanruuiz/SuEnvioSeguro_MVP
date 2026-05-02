namespace SuEnvioSeguro.API.Services.Strategies
{
    public class EstrategiaTarifaBarbosa : ITarifaMunicipioStrategy
    {
        public string Municipio => "Barbosa";

        public float CalcularTarifaBase() => 10000f;
    }
}

namespace SuEnvioSeguro.API.Services.Strategies
{
    public class EstrategiaTarifaMedellin : ITarifaMunicipioStrategy
    {
        public string Municipio => "Medellín";

        public float CalcularTarifaBase() => 5000f;
    }
}
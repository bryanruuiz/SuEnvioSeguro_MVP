namespace SuEnvioSeguro.API.Services.Strategies
{
    public class EstrategiaTarifaEnvigado : ITarifaMunicipioStrategy
    {
        public string Municipio => "Envigado";

        public float CalcularTarifaBase() => 6000f;
    }
}

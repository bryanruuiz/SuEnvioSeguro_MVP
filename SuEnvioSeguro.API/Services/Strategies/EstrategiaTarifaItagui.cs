namespace SuEnvioSeguro.API.Services.Strategies
{
    public class EstrategiaTarifaItagui : ITarifaMunicipioStrategy
    {
        public string Municipio => "Itagüí";

        public float CalcularTarifaBase() => 6000f;
    }
}

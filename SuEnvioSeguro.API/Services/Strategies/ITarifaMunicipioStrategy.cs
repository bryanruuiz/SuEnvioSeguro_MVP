namespace SuEnvioSeguro.API.Services.Strategies
{
    public interface ITarifaMunicipioStrategy 
    {
        string Municipio { get; }

        float CalcularTarifaBase();
    }
}
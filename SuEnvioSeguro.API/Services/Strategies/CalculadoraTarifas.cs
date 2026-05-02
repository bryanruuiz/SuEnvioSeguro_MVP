using SuEnvioSeguro.API.Exceptions;
using SuEnvioSeguro.API.Shared;

namespace SuEnvioSeguro.API.Services.Strategies
{
    public class CalculadoraTarifas 
    {
        private readonly IReadOnlyDictionary<string, ITarifaMunicipioStrategy> _estrategias;

        public CalculadoraTarifas(IEnumerable<ITarifaMunicipioStrategy> estrategias)
        {
            _estrategias = estrategias.ToDictionary(
                estrategia => NormalizadorTexto.NormalizarClave(estrategia.Municipio),
                estrategia => estrategia);
        }

        public float EjecutarCalculo(ITarifaMunicipioStrategy estrategia) 
        {
            return estrategia.CalcularTarifaBase();
        }

        public float CalcularTarifaMunicipio(string municipioDestino)
        {
            var claveMunicipio = NormalizadorTexto.NormalizarClave(municipioDestino);

            if (!_estrategias.TryGetValue(claveMunicipio, out var estrategia))
            {
                throw new BusinessRuleException($"No existe una estrategia de tarifa configurada para el municipio '{municipioDestino}'.");
            }

            return EjecutarCalculo(estrategia);
        }

        public float CalcularRecargoPeso(float peso)
        {
            return peso switch
            {
                >= 1f and <= 3f => 2000f,
                > 3f and <= 6f => 3000f,
                > 6f and <= 10f => 4000f,
                > 10f and <= 15f => 6000f,
                > 15f and <= 30f => 10000f,
                _ => throw new BusinessRuleException("El peso debe estar entre 1kg y 30kg para calcular la tarifa.")
            };
        }
    }
}
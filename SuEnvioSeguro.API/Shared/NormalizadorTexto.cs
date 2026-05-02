using System.Globalization;
using System.Text;

namespace SuEnvioSeguro.API.Shared
{
    public static class NormalizadorTexto
    {
        public static string NormalizarClave(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return string.Empty;
            }

            var valorNormalizado = valor.Trim().Normalize(NormalizationForm.FormD);
            var constructor = new StringBuilder(valorNormalizado.Length);

            foreach (var caracter in valorNormalizado)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(caracter) != UnicodeCategory.NonSpacingMark)
                {
                    constructor.Append(char.ToUpperInvariant(caracter));
                }
            }

            return constructor.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
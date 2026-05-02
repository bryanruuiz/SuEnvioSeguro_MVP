namespace SuEnvioSeguro.API.Models
{
    public class Cliente : Persona
    {
        public string TipoCliente { get; set; } = string.Empty;
        
        // Relación: Un cliente tiene muchas facturas[cite: 1]
        public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
    }
}
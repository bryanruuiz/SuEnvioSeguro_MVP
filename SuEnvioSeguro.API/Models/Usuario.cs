using System.ComponentModel.DataAnnotations;

namespace SuEnvioSeguro.API.Models
{
    public class Usuario : Persona
    {
        [Required]
        public string IdUsuario { get; set; } = string.Empty;
        
        [Required]
        public string Contrasena { get; set; } = string.Empty; // Aquí guardaremos el Hash (RNF-01)[cite: 1]
        
        [Required]
        public string Rol { get; set; } = string.Empty;

        // Relación: Un usuario atiende/genera muchas facturas[cite: 1]
        public virtual ICollection<Factura> FacturasGeneradas { get; set; } = new List<Factura>();
    }
}
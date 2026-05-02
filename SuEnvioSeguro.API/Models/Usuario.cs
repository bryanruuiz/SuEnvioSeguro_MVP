using System.ComponentModel.DataAnnotations;

namespace SuEnvioSeguro.API.Models
{
    public class Usuario : Persona
    {
        [Required]
        [MaxLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;
        
        [Required]
        public string Contrasena { get; set; } = string.Empty; // Aquí guardaremos el Hash (RNF-01)[cite: 1]
        
        [Required]
        [MaxLength(20)]
        public string Rol { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        // Relación: Un usuario atiende/genera muchas facturas[cite: 1]
        public virtual ICollection<Factura> FacturasGeneradas { get; set; } = new List<Factura>();
    }
}
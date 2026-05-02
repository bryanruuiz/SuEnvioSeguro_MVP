using System.ComponentModel.DataAnnotations;

namespace SuEnvioSeguro.API.Models
{
    public abstract class Persona
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string DocumentoIdentidad { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Correo { get; set; } = string.Empty;
    }
}
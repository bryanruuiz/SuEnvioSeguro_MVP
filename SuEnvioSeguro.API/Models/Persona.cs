using System.ComponentModel.DataAnnotations;

namespace SuEnvioSeguro.API.Models
{
    public abstract class Persona
    {
        [Key]
        public string DocumentoIdentidad { get; set; } = string.Empty;
        
        [Required]
        public string Nombre { get; set; } = string.Empty;
        
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
    }
}
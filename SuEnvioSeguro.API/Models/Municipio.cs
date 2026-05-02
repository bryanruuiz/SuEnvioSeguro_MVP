using System.ComponentModel.DataAnnotations;

namespace SuEnvioSeguro.API.Models
{
    public class Municipio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public float TarifaBase { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = new DateTime(2026, 5, 2);
    }
}

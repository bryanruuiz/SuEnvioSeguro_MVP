using System.ComponentModel.DataAnnotations;

namespace SuEnvioSeguro.API.Models
{
    public class Factura
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string CodigoFactura { get; set; } = string.Empty;
        
        public DateTime Fecha { get; set; } = DateTime.Now;
        public float ValorNeto { get; set; }
        public float PorcentajeIVA { get; set; }
        public float TotalAPagar { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ClienteDocumento { get; set; } = string.Empty;

        public virtual Cliente Cliente { get; set; } = null!;

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(50)]
        public string UsuarioDocumento { get; set; } = string.Empty;

        public virtual Usuario Usuario { get; set; } = null!;

        // Relación: Una factura contiene muchos envíos (1..*)[cite: 1]
        public virtual ICollection<Envio> Envios { get; set; } = new List<Envio>();
    }
}
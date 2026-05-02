using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuEnvioSeguro.API.Models
{
    public class Factura
    {
        [Key]
        public string CodigoFactura { get; set; } = string.Empty;
        
        public DateTime Fecha { get; set; } = DateTime.Now;
        public float ValorNeto { get; set; }
        public float PorcentajeIVA { get; set; }
        public float TotalAPagar { get; set; }

        // Llaves foráneas
        [Required]
        public string ClienteDocumento { get; set; } = string.Empty;
        [ForeignKey("ClienteDocumento")]
        public virtual Cliente Cliente { get; set; } = null!;

        [Required]
        public string UsuarioDocumento { get; set; } = string.Empty;
        [ForeignKey("UsuarioDocumento")]
        public virtual Usuario Usuario { get; set; } = null!;

        // Relación: Una factura contiene muchos envíos (1..*)[cite: 1]
        public virtual ICollection<Envio> Envios { get; set; } = new List<Envio>();
    }
}
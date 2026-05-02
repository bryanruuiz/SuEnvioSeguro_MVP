using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuEnvioSeguro.API.Models
{
    public class Envio
    {
        [Key]
        public string CodigoEnvio { get; set; } = string.Empty;
        
        public string DescripcionContenido { get; set; } = string.Empty;
        public string MunicipioDestino { get; set; } = string.Empty;
        public float Peso { get; set; }
        public int Cantidad { get; set; }
        public string DireccionEnvio { get; set; } = string.Empty;
        public bool EsDelicado { get; set; }
        public float ValorNetoEnvio { get; set; }
        
        public string Estado { get; set; } = "REGISTRADO";

        // Relación: Un envío pertenece a una factura
        [Required]
        public string CodigoFactura { get; set; } = string.Empty;
        [ForeignKey("CodigoFactura")]
        public virtual Factura Factura { get; set; } = null!;
    }
}
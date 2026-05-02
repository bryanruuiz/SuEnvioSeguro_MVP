using System.ComponentModel.DataAnnotations;

namespace SuEnvioSeguro.API.Models
{
    public class Envio
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string CodigoEnvio { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string DescripcionContenido { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string MunicipioDestino { get; set; } = string.Empty;

        public float Peso { get; set; }
        public int Cantidad { get; set; }

        [Required]
        [MaxLength(200)]
        public string DireccionEnvio { get; set; } = string.Empty;

        public bool EsDelicado { get; set; }
        public float ValorNetoEnvio { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "REGISTRADO";

        [Required]
        public int FacturaId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CodigoFactura { get; set; } = string.Empty;

        public virtual Factura Factura { get; set; } = null!;
    }
}
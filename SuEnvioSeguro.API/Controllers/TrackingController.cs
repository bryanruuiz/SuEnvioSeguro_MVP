using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuEnvioSeguro.API.Data;

namespace SuEnvioSeguro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrackingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{codigoEnvio}")]
        public async Task<IActionResult> ConsultarEstado(string codigoEnvio)
        {
            var envio = await _context.Envios
                .Include(e => e.Factura)
                .ThenInclude(f => f.Cliente)
                .FirstOrDefaultAsync(e => e.CodigoEnvio == codigoEnvio);

            if (envio == null)
                return NotFound(new { message = "Envío no encontrado" });

            return Ok(new
            {
                idEnvio = envio.Id,
                codigoEnvio = envio.CodigoEnvio,
                codigoFactura = envio.CodigoFactura,
                idFactura = envio.FacturaId,
                descripcion = envio.DescripcionContenido,
                origen = "Medellín", // Puede ser dinámico según requieran
                destino = envio.MunicipioDestino,
                direccionEntrega = envio.DireccionEnvio,
                peso = envio.Peso,
                cantidad = envio.Cantidad,
                esDelicado = envio.EsDelicado,
                valorNeto = envio.ValorNetoEnvio,
                valorTotal = envio.Factura?.TotalAPagar,
                estado = envio.Estado,
                fechaRegistro = envio.Factura?.Fecha
            });
        }

        [HttpGet("codigo-factura/{codigoFactura}")]
        public async Task<IActionResult> ConsultarPorFactura(string codigoFactura)
        {
            var factura = await _context.Facturas
                .Include(f => f.Envios)
                .Include(f => f.Cliente)
                .FirstOrDefaultAsync(f => f.CodigoFactura == codigoFactura);

            if (factura == null)
                return NotFound(new { message = "Factura no encontrada" });

            var envios = factura.Envios.Select(e => new
            {
                idEnvio = e.Id,
                codigoEnvio = e.CodigoEnvio,
                descripcion = e.DescripcionContenido,
                destino = e.MunicipioDestino,
                estado = e.Estado,
                valor = e.ValorNetoEnvio
            });

            return Ok(new
            {
                idFactura = factura.Id,
                codigoFactura = factura.CodigoFactura,
                cliente = factura.Cliente?.Nombre,
                fecha = factura.Fecha,
                valorNeto = factura.ValorNeto,
                iva = factura.PorcentajeIVA * 100,
                totalAPagar = factura.TotalAPagar,
                envios = envios
            });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TransaccionesService.DTO;
using TransaccionesService.Models;
using TransaccionesService.Services;

namespace TransaccionesService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionController : ControllerBase
    {
        private readonly ITransaccionService _transaccionService;
        private readonly ILogger<TransaccionController> _logger;

        public TransaccionController(ILogger<TransaccionController> logger,
                                  ITransaccionService transaccionService)
        {
            _logger = logger;
            _transaccionService = transaccionService;
        }

        [HttpGet("getTransacciones")]
        public ActionResult<List<Transaccion>> getProductos()
        {
            return Ok(_transaccionService.GetAll());
        }

        [HttpGet("getTransaccionById/{id}")]
        public ActionResult<Transaccion> getProductoById(int id)
        {
            var producto = _transaccionService.GetById(id);

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        [HttpPost("registrarTransaccion")]
        public ActionResult registrarTransaccion([FromBody] Transaccion transaccion)
        {
            var result = _transaccionService.RegistrarTransaccion(transaccion);

            if (result == null)
                return NotFound();

            return result.Result ? Ok() : NotFound();
        }

        [HttpPut("actualizarTransaccion/{id}")]
        public ActionResult actualizarProducto(int id, [FromBody] Transaccion transaccion)
        {
            var result = _transaccionService.Update(transaccion);

            return result ? Ok() : NotFound();
        }

        [HttpDelete("removerTransaccion/{id}")]
        public ActionResult removerProducto(int id)
        {
            var result = _transaccionService.Delete(id);

            return result ? Ok() : NotFound();
        }

        [HttpPost("obtenerHistorial")]
        public async Task<ActionResult<HistorialProductoDTO>> obtenerHistorial([FromBody] HistorialRequest historialRequest)
        {
            var result = await _transaccionService.ObtenerHistorialProductoAsync(historialRequest);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductosService.Models;
using ProductosService.Services;

namespace ProductosService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(ILogger<ProductoController> logger,
                                  IProductoService productoService)
        {
            _logger = logger;
            _productoService = productoService;
        }

        [HttpGet("getProductos")]
        public ActionResult<List<Producto>> getProductos()
        {
            return Ok(_productoService.GetAll());
        }

        [HttpGet("getProductoById/{id}")]
        public ActionResult<Producto> getProductoById(int id)
        {
            var producto = _productoService.GetById(id);

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        [HttpPost("crearProducto")]
        public ActionResult crearProducto([FromBody] Producto producto)
        {
            var result = _productoService.Create(producto);
             
            return result ? Ok() : BadRequest();
        }

        [HttpPut("actualizarProducto/{id}")]
        public ActionResult actualizarProducto(int id, [FromBody] Producto producto)
        {
            var result = _productoService.Update(id, producto);

            return result ? Ok() : NotFound();
        }

        [HttpDelete("removerProducto/{id}")]
        public ActionResult removerProducto(int id)
        {
            var result = _productoService.Delete(id);

            return result ? Ok() : NotFound();
        }
    }
}

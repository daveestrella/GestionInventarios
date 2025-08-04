using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TransaccionesService.Data;
using TransaccionesService.DTO;
using TransaccionesService.Models;

namespace TransaccionesService.Services
{
    public class TransaccionService : ITransaccionService
    {
        private readonly TransaccionDbContext _context;
        private readonly HttpClient _httpClient;

        public TransaccionService(TransaccionDbContext context, IHttpClientFactory factory)
        {
            _context = context;
            _httpClient = factory.CreateClient("ProductosService");
        }

        public List<Transaccion> GetAll()
        {
            List<Transaccion> result = new List<Transaccion>();

            try
            {
                result = _context.Transacciones.OrderByDescending(t => t.Fecha).ToList();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public Transaccion GetById(int id)
        {
            Transaccion result = new Transaccion();

            try
            {
                result = _context.Transacciones.Find(id);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<bool> RegistrarTransaccion(Transaccion transaccion)
        {
            bool result = false;

            try
            {
                var response = await _httpClient.GetAsync($"/api/Producto/getProductoById/{transaccion.ProductoId}");

                var json = await response.Content.ReadAsStringAsync();

                var producto = JsonSerializer.Deserialize<ProductoDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (producto == null)
                    throw new Exception("Producto no encontrado.");

                if (transaccion.Tipo == "VENTA")
                {
                    if (producto.Stock < transaccion.Cantidad)
                        throw new Exception("Stock insuficiente para realizar la venta.");

                    producto.Stock -= transaccion.Cantidad;
                }
                else if (transaccion.Tipo == "COMPRA")
                {
                    producto.Stock += transaccion.Cantidad;
                }
                else
                {
                    throw new Exception("Tipo de transacción inválido (solo COMPRA o VENTA).");
                }

                transaccion.PrecioTotal = transaccion.Cantidad * transaccion.PrecioUnitario;
                transaccion.Fecha = DateTime.UtcNow;

                var updateResponse = await _httpClient.PutAsJsonAsync($"/api/Producto/actualizarProducto/{producto.Id}", producto);
                updateResponse.EnsureSuccessStatusCode();

                _context.Transacciones.Add(transaccion);
                _context.SaveChanges();

                result = true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public bool Update(Transaccion transaccion)
        {
            bool result = false;

            try
            {
                var existing = _context.Transacciones.Find(transaccion.Id);

                if (existing == null)
                {
                    Console.WriteLine("No se encuentra el producto con id " + transaccion.Id);
                    return false;
                }

                existing.Fecha = transaccion.Fecha;
                existing.Tipo = transaccion.Tipo;
                existing.ProductoId = transaccion.ProductoId;
                existing.Cantidad = transaccion.Cantidad;
                existing.PrecioUnitario = transaccion.PrecioUnitario;
                existing.PrecioTotal = transaccion.PrecioTotal;
                existing.Detalle = transaccion.Detalle;

                _context.SaveChanges();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public bool Delete(int id)
        {
            bool result = false;

            try
            {
                var transaccion = _context.Transacciones.Find(id);

                if (transaccion == null)
                {
                    Console.WriteLine("No se encuentra la transaccion con id " + transaccion.Id);
                    return false;
                }

                _context.Transacciones.Remove(transaccion);
                _context.SaveChanges();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public async Task<HistorialProductoDTO> ObtenerHistorialProductoAsync(HistorialRequest historialRequest)
        {
            HistorialProductoDTO result = new HistorialProductoDTO();

            try
            {
                var response = await _httpClient.GetAsync($"/api/Producto/getProductoById/{historialRequest.productoId}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();

                var producto = JsonSerializer.Deserialize<ProductoDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (producto == null)
                    throw new Exception("Producto no encontrado.");

                var query = _context.Transacciones
                            .Where(t => t.ProductoId == historialRequest.productoId)
                            .AsQueryable();

                if (historialRequest.fechaInicio.HasValue)
                    query = query.Where(t => t.Fecha >= historialRequest.fechaInicio.Value);

                if (historialRequest.fechaFin.HasValue)
                    query = query.Where(t => t.Fecha <= historialRequest.fechaFin.Value);

                if (!string.IsNullOrEmpty(historialRequest.tipoTransaccion))
                    query = query.Where(t => t.Tipo.ToUpper() == historialRequest.tipoTransaccion.ToUpper());

                var transacciones = await query
                    .OrderByDescending(t => t.Fecha)
                    .ToListAsync();

                result = new HistorialProductoDTO
                {
                    Nombre = producto.Nombre,
                    Stock = producto.Stock,
                    Transacciones = transacciones
                        .OrderByDescending(t => t.Fecha)
                        .Select(t => new TransaccionDTO
                        {
                            Fecha = t.Fecha,
                            Tipo = t.Tipo,
                            Cantidad = t.Cantidad,
                            PrecioUnitario = t.PrecioUnitario,
                            PrecioTotal = t.PrecioTotal,
                            Detalle = t.Detalle
                        }).ToList()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }
    }
}

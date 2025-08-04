using ProductosService.Data;
using ProductosService.Models;

namespace ProductosService.Services
{
    public class ProductoService : IProductoService
    {
        private readonly ProductoDbContext _context;

        public ProductoService(ProductoDbContext context)
        {
            _context = context;
        }

        public List<Producto> GetAll()
        {
            List<Producto> result = new List<Producto>();

            try
            {
                result = _context.Productos.ToList();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public Producto GetById(int id)
        {
            Producto result = new Producto();

            try
            {
                result = _context.Productos.Find(id);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public bool Create(Producto producto)
        {
            bool result = false;

            try
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();

                result = true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public bool Update(int productId, Producto producto)
        {
            bool result = false;

            try
            {
                var existing = _context.Productos.Find(productId);

                if (existing == null)
                {
                    Console.WriteLine("No se encuentra el producto con id " + producto.Id);
                    return false;
                }

                existing.Nombre = producto.Nombre;
                existing.Descripcion = producto.Descripcion;
                existing.Categoria = producto.Categoria;
                existing.ImagenUrl = producto.ImagenUrl;
                existing.Precio = producto.Precio;
                existing.Stock = producto.Stock;

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
                var producto = _context.Productos.Find(id);

                if (producto == null)
                {
                    Console.WriteLine("No se encuentra el producto con id " + producto.Id);
                    return false;
                }

                _context.Productos.Remove(producto);
                _context.SaveChanges();

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }
    }
}

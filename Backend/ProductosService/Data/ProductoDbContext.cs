using Microsoft.EntityFrameworkCore;
using ProductosService.Models;

namespace ProductosService.Data
{
    public class ProductoDbContext : DbContext
    {
        public ProductoDbContext(DbContextOptions<ProductoDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
    }

}

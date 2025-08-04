using Microsoft.EntityFrameworkCore;
using TransaccionesService.Models;

namespace TransaccionesService.Data
{
    public class TransaccionDbContext : DbContext
    {
        public TransaccionDbContext(DbContextOptions<TransaccionDbContext> options) : base(options) { }

        public DbSet<Transaccion> Transacciones { get; set; }
    }
}

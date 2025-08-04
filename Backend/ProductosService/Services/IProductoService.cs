using Microsoft.AspNetCore.Mvc;
using ProductosService.Models;

namespace ProductosService.Services
{
    public interface IProductoService
    {
        List<Producto> GetAll();
        Producto? GetById(int id);
        bool Create(Producto producto);
        bool Update(int productId, Producto producto);
        bool Delete(int id);
    }
}

using TransaccionesService.DTO;
using TransaccionesService.Models;

namespace TransaccionesService.Services
{
    public interface ITransaccionService
    {
        List<Transaccion> GetAll();
        Transaccion? GetById(int id);
        Task<bool> RegistrarTransaccion(Transaccion transaccion);
        bool Update(Transaccion transaccion);
        bool Delete(int id);
        Task<HistorialProductoDTO> ObtenerHistorialProductoAsync(HistorialRequest historialRequest);
    }
}

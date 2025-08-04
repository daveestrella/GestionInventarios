namespace TransaccionesService.DTO
{
    public class HistorialProductoDTO
    {
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public List<TransaccionDTO> Transacciones { get; set; }
    }
}

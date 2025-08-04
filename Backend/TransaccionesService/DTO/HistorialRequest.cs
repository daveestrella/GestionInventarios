namespace TransaccionesService.DTO
{
    public class HistorialRequest
    {
        public int productoId { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public string? tipoTransaccion { get; set; }
    }
}

namespace TransaccionesService.DTO
{
    public class TransaccionDTO
    {
        public int ProductoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Detalle { get; set; }
    }
}

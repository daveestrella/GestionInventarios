using System.ComponentModel.DataAnnotations.Schema;

namespace TransaccionesService.Models
{
    [Table("productos")]
    public class Producto
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("categoria")]
        public string Categoria { get; set; }

        [Column("imagen_url")]
        public string? ImagenUrl { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }

        [Column("stock")]
        public int Stock { get; set; }
    }

}

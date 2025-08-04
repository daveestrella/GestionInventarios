﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TransaccionesService.Models
{
    [Table("transacciones")]
    public class Transaccion
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; }

        [Column("producto_id")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("precio_unitario")]
        public decimal PrecioUnitario { get; set; }

        [Column("precio_total")]
        public decimal PrecioTotal { get; set; }

        [Column("detalle")]
        public string? Detalle { get; set; }
    }
}

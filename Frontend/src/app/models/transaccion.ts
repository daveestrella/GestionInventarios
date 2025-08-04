export interface Transaccion {
  id?: number;
  fecha?: string;
  tipo: 'COMPRA' | 'VENTA';
  productoId: number;
  cantidad: number;
  precioUnitario: number;
  precioTotal?: number;
  detalle?: string;
  productoNombre?: string;
}

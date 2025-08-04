export interface HistorialRequest {
    productoId: number;
    fechaInicio?: string;
    fechaFin?: string;
    tipoTransaccion?: 'COMPRA' | 'VENTA';
}

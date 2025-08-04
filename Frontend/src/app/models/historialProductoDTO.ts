import { Transaccion } from "./transaccion";

export interface HistorialProductoDTO {
  nombre: string;
  stock: number;
  transacciones: Transaccion[];
}

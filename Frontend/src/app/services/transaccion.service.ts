import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Producto } from '../models/producto';
import { Observable } from 'rxjs';
import { Transaccion } from '../models/transaccion';
import { HistorialRequest } from '../models/historialRequest';
import { HistorialProductoDTO } from '../models/historialProductoDTO';

@Injectable({
  providedIn: 'root'
})
export class TransaccionService {

  private readonly apiUrl = 'https://localhost:7104/api/Transaccion';

  constructor(private http: HttpClient) { }

  getTransacciones(): Observable<Transaccion[]> {
    return this.http.get<Transaccion[]>(`${this.apiUrl}/getTransacciones`);
  }

  getTransaccion(id: number): Observable<Transaccion> {
    return this.http.get<Transaccion>(`${this.apiUrl}/getTransaccionById/${id}`);
  }

  registrarTransaccion(producto: Transaccion): Observable<any> {
    return this.http.post(`${this.apiUrl}/registrarTransaccion`, producto);
  }

  actualizarTransaccion(id: number, producto: Transaccion): Observable<any> {
    return this.http.put(`${this.apiUrl}/actualizarTransaccion/${id}`, producto);
  }

  removerTransaccion(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/removerTransaccion/${id}`);
  }

  obtenerHistorial(request: HistorialRequest): Observable<any> {
    return this.http.post<HistorialProductoDTO>(`${this.apiUrl}/obtenerHistorial`, request);
  }
}

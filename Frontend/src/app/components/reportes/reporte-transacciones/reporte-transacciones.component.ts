import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { Producto } from '../../../models/producto';
import { HistorialProductoDTO } from '../../../models/historialProductoDTO';
import { ProductoService } from '../../../services/producto.service';
import { TransaccionService } from '../../../services/transaccion.service';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { HistorialRequest } from '../../../models/historialRequest';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-reporte-transacciones',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatCardModule,
  ],
  templateUrl: './reporte-transacciones.component.html',
  styleUrl: './reporte-transacciones.component.scss'
})
export class ReporteTransaccionesComponent implements OnInit {

  form!: FormGroup;
  productos: Producto[] = [];
  historial: HistorialProductoDTO | null = null;
  displayedColumns = ['fecha', 'tipo', 'cantidad', 'precioUnitario', 'precioTotal', 'detalle'];

  constructor(
    private fb: FormBuilder,
    private productoService: ProductoService,
    private transaccionService: TransaccionService
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      productoId: [null],
      fechaInicio: [null],
      fechaFin: [null],
      tipoTransaccion: ['']
    });

    this.productoService.getProductos().subscribe({
      next: (productos) => (this.productos = productos),
    });
  }

  buscar(): void {
    const raw = this.form.value;

    const request: HistorialRequest = {
      productoId: raw.productoId,
      fechaInicio: raw.fechaInicio ? raw.fechaInicio.toISOString() : undefined,
      fechaFin: raw.fechaFin ? raw.fechaFin.toISOString() : undefined,
      tipoTransaccion: raw.tipoTransaccion || undefined
    };

    this.transaccionService.obtenerHistorial(request).subscribe({
      next: historial => this.historial = historial
    });
  }

  formatFecha(fecha: string): string {
    const date = new Date(fecha);
    return date.toLocaleDateString('es-ES', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: false
    });
  }
}

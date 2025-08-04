import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorIntl, MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { TransaccionService } from '../../../services/transaccion.service';
import { ProductoService } from '../../../services/producto.service';
import { Producto } from '../../../models/producto';
import { Transaccion } from '../../../models/transaccion';
import { TransaccionFormComponent } from '../transaccion-form/transaccion-form.component';
import { MatSort } from '@angular/material/sort';
import { MatCard, MatCardContent, MatCardModule } from "@angular/material/card";
import { getEspPaginatorIntl } from '../../../app.component';
import { catchError, forkJoin, map, of } from 'rxjs';

@Component({
  selector: 'app-transacciones-list',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatSnackBarModule
  ],
  templateUrl: './transacciones-list.component.html',
  styleUrl: './transacciones-list.component.scss',
  providers: [
    { provide: MatPaginatorIntl, useValue: getEspPaginatorIntl() },
  ],
})
export class TransaccionesListComponent implements OnInit {
  displayedColumns: string[] = [
    'fecha',
    'tipo',
    'producto',
    'cantidad',
    'precioUnitario',
    'precioTotal',
    'detalle'
  ];

  transacciones: Transaccion[] = [];
  dataSource = new MatTableDataSource<Transaccion>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private transaccionService: TransaccionService,
    private productoService: ProductoService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.cargarTransacciones();
  }

  cargarTransacciones(): void {
    this.transaccionService.getTransacciones().subscribe({
      next: (transacciones) => {
        const observables = transacciones.map(t =>
          this.productoService.getProducto(t.productoId).pipe(
            map(producto => ({
              ...t,
              productoNombre: producto.nombre
            })),
            catchError(() => of({
              ...t,
              productoNombre: 'Desconocido'
            }))
          )
        );

        forkJoin(observables).subscribe({
          next: transaccionesConProducto => {
            this.transacciones = transaccionesConProducto;

            this.dataSource.data = this.transacciones;
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
          }
        });
      }
    });
  }

  abrirDialogoRegistrar(): void {
    const dialogRef = this.dialog.open(TransaccionFormComponent, {
      width: '600px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.cargarTransacciones();
      }
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

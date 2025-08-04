import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { Producto } from '../../../models/producto';
import { ProductoService } from '../../../services/producto.service';
import { TransaccionService } from '../../../services/transaccion.service';

@Component({
  selector: 'app-transaccion-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatDialogModule
  ],
  templateUrl: './transaccion-form.component.html',
  styleUrl: './transaccion-form.component.scss'
})
export class TransaccionFormComponent implements OnInit {

  form!: FormGroup;
  productos: Producto[] = [];
  stockDisponible: number | null = null;
  precioTotal: number = 0;

  constructor(
    private fb: FormBuilder,
    private transaccionService: TransaccionService,
    private productoService: ProductoService,
    private snackbar: MatSnackBar,
    private dialogRef: MatDialogRef<TransaccionFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      tipo: ['COMPRA', Validators.required],
      productoId: [null, Validators.required],
      cantidad: [0, [Validators.required, Validators.min(1)]],
      precioUnitario: [0, [Validators.required, Validators.min(0)]],
      detalle: ['']
    });

    this.productoService.getProductos().subscribe({
      next: productos => this.productos = productos
    });

    this.form.get('tipo')?.valueChanges.subscribe(() => this.actualizarCamposDependientes());
    this.form.get('productoId')?.valueChanges.subscribe(() => this.actualizarCamposDependientes());

    this.form.get('cantidad')?.valueChanges.subscribe(() => this.actualizarPrecioTotal());
    this.form.get('precioUnitario')?.valueChanges.subscribe(() => this.actualizarPrecioTotal());

  }

  actualizarCamposDependientes(): void {
    const tipo = this.form.get('tipo')?.value;
    const productoId = this.form.get('productoId')?.value;
    const producto = this.productos.find(p => p.id === productoId);

    if (!producto) {
      this.stockDisponible = null;
      return;
    }

    // Set precio automáticamente
    this.form.get('precioUnitario')?.setValue(producto.precio);

    // Validaciones para cantidad
    const validators = [Validators.required, Validators.min(1)];
    this.stockDisponible = null;

    if (tipo === 'VENTA') {
      validators.push(Validators.max(producto.stock));
      this.stockDisponible = producto.stock;
    }

    this.form.get('cantidad')?.setValidators(validators);
    this.form.get('cantidad')?.updateValueAndValidity();
  }

  actualizarPrecioTotal(): void {
    const cantidad = this.form.get('cantidad')?.value || 0;
    const precioUnitario = this.form.get('precioUnitario')?.value || 0;
    this.precioTotal = cantidad * precioUnitario;
  }

  guardar(): void {
    if (this.form.invalid) return;

    const transaccion = this.form.value;
    this.transaccionService.registrarTransaccion(transaccion).subscribe({
      next: () => {
        this.snackbar.open('Transacción registrada exitosamente', 'Cerrar', { duration: 3000 });
        this.dialogRef.close(true);
      },
      error: () =>
        this.snackbar.open('Error al registrar la transacción', 'Cerrar', { duration: 3000 })
    });
  }

  cancelar(): void {
    this.dialogRef.close();
  }
}

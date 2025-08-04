import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Producto } from '../../../models/producto';
import { ProductoService } from '../../../services/producto.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-producto-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    MatCardModule
  ],
  templateUrl: './producto-form.component.html',
  styleUrl: './producto-form.component.scss'
})
export class ProductoFormComponent implements OnInit {

  form!: FormGroup;
  esEdicion = false;
  productoId?: number;

  constructor(
    private fb: FormBuilder,
    private productoService: ProductoService,
    private snackbar: MatSnackBar,
    private dialogRef: MatDialogRef<ProductoFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { productoId?: number }
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      nombre: ['', Validators.required],
      descripcion: [''],
      categoria: [''],
      imagenUrl: [''],
      precio: [0, [Validators.required, Validators.min(0)]],
      stock: [0, [Validators.required, Validators.min(0)]]
    });

    if (this.data?.productoId) {
      this.esEdicion = true;
      this.productoId = this.data.productoId;
      this.cargarProducto(this.productoId);
    }
  }

  cargarProducto(id: number): void {
    this.productoService.getProducto(id).subscribe({
      next: (producto) => this.form.patchValue(producto),
      error: () => this.snackbar.open('No se pudo cargar el producto', 'Cerrar', { duration: 3000 })
    });
  }

  close(){
    this.dialogRef.close();
  }

  guardar(): void {
    if (this.form.invalid) return;

    const producto = this.form.value as Producto;

    const peticion = this.esEdicion
      ? this.productoService.actualizarProducto(this.productoId!, producto)
      : this.productoService.crearProducto(producto);

    peticion.subscribe({
      next: () => {
        this.snackbar.open(
          `Producto ${this.esEdicion ? 'actualizado' : 'creado'} exitosamente`,
          'Cerrar',
          { duration: 3000 }
        );
        this.dialogRef.close(true);
      },
      error: () =>
        this.snackbar.open('Error al guardar el producto', 'Cerrar', { duration: 3000 })
    });
  }
}

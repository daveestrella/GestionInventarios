import { Routes } from '@angular/router';
import { MainLayoutComponent } from './components/layout/main-layout/main-layout.component';
import { ProductosListComponent } from './components/productos/productos-list/productos-list.component';
import { TransaccionesListComponent } from './components/transacciones/transacciones-list/transacciones-list.component';
import { ProductoFormComponent } from './components/productos/producto-form/producto-form.component';
import { ReporteTransaccionesComponent } from './components/reportes/reporte-transacciones/reporte-transacciones.component';

export const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: 'productos', component: ProductosListComponent },
      { path: 'productos/nuevo', component: ProductoFormComponent },
      { path: 'productos/editar/:id', component: ProductoFormComponent },

      { path: 'transacciones', component: TransaccionesListComponent },
      { path: 'transacciones/registrar', component: ProductoFormComponent },
      { path: 'transacciones/editar/:id', component: ProductoFormComponent },

      { path: 'reporte', component: ReporteTransaccionesComponent },

      { path: '', redirectTo: 'productos', pathMatch: 'full' }
    ]
  }
];

# GestionInventarios

Este proyecto es una aplicación web desarrollada con arquitectura de microservicios. Permite la gestión de productos y el control de transacciones de inventario (compras y ventas), con backend en **.NET Core** y frontend en **Angular**. La base de datos utilizada es **PostgreSQL**.

---

## ✅ Requisitos

### Generales
- [Git](https://git-scm.com/)
- [Node.js (v18+ recomendado)](https://nodejs.org/)
- [npm](https://www.npmjs.com/) o [Yarn](https://yarnpkg.com/)
- [Angular CLI](https://angular.io/cli)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) con:
  - .NET 7 SDK o superior
  - C# y ASP.NET Core workloads
- [PostgreSQL 14+](https://www.postgresql.org/)

---

## 🚀 Ejecución del Backend (.NET Core)

El backend está compuesto por dos microservicios:
- `ProductosService` → CRUD de productos
- `TransaccionesService` → Registro de transacciones y reportes

### 1. Configurar la base de datos

Ejecutar este script en tu base de datos PostgreSQL:

```sql
-- Crear la tabla de productos
CREATE TABLE productos (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    categoria VARCHAR(100),
    imagen_url TEXT,
    precio NUMERIC(10, 2) NOT NULL CHECK (precio >= 0),
    stock INTEGER NOT NULL CHECK (stock >= 0)
);

-- Crear la tabla de transacciones
CREATE TABLE transacciones (
    id SERIAL PRIMARY KEY,
    fecha TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    tipo VARCHAR(10) NOT NULL CHECK (tipo IN ('COMPRA', 'VENTA')),
    producto_id INTEGER NOT NULL REFERENCES productos(id) ON DELETE CASCADE,
    cantidad INTEGER NOT NULL CHECK (cantidad > 0),
    precio_unitario NUMERIC(10, 2) NOT NULL CHECK (precio_unitario >= 0),
    precio_total NUMERIC(12, 2) NOT NULL CHECK (precio_total >= 0),
    detalle TEXT
);
```

> Asegúrate de que el nombre de la base de datos, usuario y contraseña coincidan con la cadena de conexión en `appsettings.json` de cada microservicio.

### 2. Ejecutar los microservicios

Desde Visual Studio o la terminal, ejecutar:

```bash
# Desde la raíz de cada microservicio
cd ProductosService
dotnet run

cd ../TransaccionesService
dotnet run
```

> Ambos servicios expondrán Swagger UI en sus respectivos puertos, por ejemplo:  
> `https://localhost:7111/swagger` y `https://localhost:7104/swagger`.

---

## 🌐 Ejecución del Frontend (Angular)

### 1. Instalar dependencias

```bash
cd frontend
npm install
```

### 2. Ejecutar la aplicación

```bash
ng serve
```

La aplicación estará disponible en:  
`http://localhost:4200`

---

## 📌 Notas adicionales

- Se debe habilitar **CORS** en los microservicios para que el frontend pueda acceder sin errores.
- El frontend usa componentes standalone y `@angular/material`.

## 🖼️ Evidencias

### Listado dinámico de productos y transacciones con paginación.
<img width="1596" height="400" alt="image" src="https://github.com/user-attachments/assets/3a28261e-c050-405d-8fe0-6cc9e0da484c" />
<img width="1595" height="558" alt="image" src="https://github.com/user-attachments/assets/e88ed7a5-3afc-40a9-952a-7a0a29a70f49" />

### Pantalla para la creación de productos.
<img width="1587" height="855" alt="image" src="https://github.com/user-attachments/assets/34a09676-61e8-4e99-a9ca-5d830cf03c0c" />

### Pantalla para la edición de productos.
<img width="1595" height="834" alt="image" src="https://github.com/user-attachments/assets/818a8c0d-736f-4ad5-ac8a-b598efb82ddb" />

### Pantalla para la creación de transacciones.
<img width="1602" height="830" alt="image" src="https://github.com/user-attachments/assets/7898b53b-f41e-4a7b-8cdc-7f2675ad92dc" />

### Pantalla para la edición de transacciones.
<img width="1602" height="830" alt="image" src="https://github.com/user-attachments/assets/253188e4-0c10-47a9-99f1-dd76ee458f74" />

### Pantalla de filtros dinámicos.
<img width="1597" height="392" alt="image" src="https://github.com/user-attachments/assets/0a1b118b-512b-484a-997a-9fb941423e0c" />

### Pantalla para la consulta de información de un formulario (extra).
<img width="1596" height="664" alt="image" src="https://github.com/user-attachments/assets/5a82d74c-8da3-4edf-86fa-62427163f5b9" />

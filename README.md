# Empanada Inventory System (CRUD)

Este es un sistema de gestión de inventario para una venta de empanadas, desarrollado como proyecto práctico para la asignatura de **Programación III**. El sistema permite realizar el ciclo completo de mantenimiento de productos (CRUD) mediante una interfaz web moderna y robusta.

## 🚀 Características principales
* **Gestión de Productos:** Registro, visualización, edición y eliminación de empanadas.
* **Arquitectura MVC:** Separación clara de responsabilidades utilizando el patrón Modelo-Vista-Controlador.
* **Persistencia de Datos:** Conexión e integración directa con **SQL Server**.
* **Flujo de Trabajo Profesional:** Implementación rigurosa de **Git Flow** con ramas `main`, `dev`, `qa`,  `hotfix/*` y `feature/*`.

## 🛠️ Tecnologías utilizadas
* **Lenguaje:** C# (.NET 9.0)
* **Framework Web:** ASP.NET Core MVC
* **ORM:** Entity Framework Core
* **Base de Datos:** Microsoft SQL Server
* **Control de Versiones:** Git & GitHub

## 📋 Requisitos para ejecución
Para correr este proyecto localmente, asegúrate de tener:
1. SDK de .NET 9.0 o superior.
2. SQL Server (LocalDB o Express).
3. Visual Studio Code o Visual Studio 2022.

## 🔧 Pasos para la Configuración y Prueba

Sigue este orden para poner en marcha el sistema en tu entorno local:

### 1. Preparación de la Base de Datos
Ejecuta el siguiente script en **SQL Server Management Studio (SSMS)** para crear la estructura y los datos iniciales:

CREATE DATABASE EmpanadaDB;
GO
USE EmpanadaDB;
GO

CREATE TABLE Empanadas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Sabor NVARCHAR(100) NOT NULL,
    Precio DECIMAL(18,2) NOT NULL,
    CantidadInventario INT NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

-- Datos iniciales para pruebas
INSERT INTO Empanadas (Sabor, Precio, CantidadInventario)
VALUES ('Pollo', 50.00, 20), ('Queso', 45.00, 15), ('Pizza', 55.00, 10);
GO

### 2. Configuración del Proyecto
Clonar el repositorio:

git clone [https://github.com/TuUsuario/Empanada-Inventory-System.git](https://github.com/TuUsuario/Empanada-Inventory-System.git)

Actualizar ConnectionString:
Modifica el archivo appsettings.json con los datos de tu servidor local:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=EmpanadaDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

Nota: Reemplazar . por el nombre de tu instancia de SQL Server si es necesario.

### 3. Ejecución
Abre una terminal en la carpeta raíz del proyecto y ejecuta los siguientes comandos:

Bash
# Restaurar paquetes de NuGet
dotnet restore

# Compilar y ejecutar la aplicación
dotnet run
Una vez que el servidor esté activo, accede en tu navegador a:

http://localhost:5000/Empanada

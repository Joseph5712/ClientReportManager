# ClientReport Manager

Sistema web administrativo para la gestión de clientes y generación de reportes, desarrollado con ASP.NET Core MVC, C#, Entity Framework Core y SQL Server.

## Descripción

ClientReport Manager es una aplicación web orientada a la administración centralizada de clientes. El sistema permite registrar, consultar, editar y desactivar clientes, además de visualizar métricas generales mediante un dashboard y generar reportes filtrados por fecha, estado, tipo de cliente y texto de búsqueda.

El proyecto fue desarrollado como una práctica integral de desarrollo web con .NET, aplicando conceptos de arquitectura MVC, separación de responsabilidades, servicios, interfaces, inyección de dependencias, validaciones, autenticación básica, reportes y criterios de experiencia de usuario.

## Objetivo del proyecto

El objetivo principal del proyecto es simular una solución administrativa real que permita gestionar información de clientes y generar reportes útiles para la toma de decisiones.

Este sistema busca resolver problemas comunes en empresas que administran información de clientes mediante hojas de cálculo, documentos separados o procesos manuales, centralizando los datos en una aplicación web con base de datos relacional.

## Tecnologías utilizadas

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQL Server
- HTML
- CSS
- JavaScript
- Bootstrap
- Autenticación por cookies
- PasswordHasher
- Git / GitHub

## Funcionalidades principales

### Autenticación

- Inicio de sesión.
- Cierre de sesión.
- Autenticación mediante cookies.
- Protección de rutas internas con `[Authorize]`.
- Manejo de contraseña con `PasswordHasher`.

### Gestión de clientes

- Registro de clientes.
- Edición de clientes.
- Consulta de detalle.
- Búsqueda por nombre, identificación, correo, teléfono o empresa.
- Filtro por estado.
- Filtro por tipo de cliente.
- Desactivación lógica de clientes.
- Validación de identificación duplicada.

### Dashboard

- Total de clientes registrados.
- Clientes activos.
- Clientes inactivos.
- Clientes potenciales.
- Clientes registrados durante el mes actual.
- Últimos clientes registrados.
- Resumen por estado.
- Resumen por tipo.

### Reportes

- Reporte general de clientes.
- Filtro por rango de fechas.
- Filtro por estado.
- Filtro por tipo de cliente.
- Búsqueda por texto.
- Resumen por estado.
- Resumen por tipo.
- Detalle del reporte.

### Exportación

- Exportación de reportes a CSV.
- Exportación respetando los filtros aplicados.
- Separador compatible con Excel en configuración regional en español.
- Manejo de caracteres especiales en el archivo CSV.

### Experiencia de usuario

- Layout administrativo con sidebar.
- Topbar con usuario autenticado.
- Formularios ordenados.
- Tablas limpias.
- Tarjetas de métricas.
- Mensajes de retroalimentación.
- Confirmaciones antes de acciones importantes.
- Diseño responsive básico.

## Arquitectura del proyecto

El proyecto utiliza el patrón MVC y una capa adicional de servicios para separar la lógica de negocio de los controladores.

La estructura principal es:

```text
Controllers/
    AccountController.cs
    ClientesController.cs
    DashboardController.cs
    ReportesController.cs

Data/
    ApplicationDbContext.cs

Models/
    Cliente.cs
    EstadoCliente.cs
    TipoCliente.cs
    Usuario.cs

Services/
    IClienteService.cs
    ClienteService.cs
    IDashboardService.cs
    DashboardService.cs
    IReporteService.cs
    ReporteService.cs
    IUsuarioService.cs
    UsuarioService.cs

ViewModels/
    ClienteFiltroViewModel.cs
    DashboardViewModel.cs
    LoginViewModel.cs
    ReporteClientesViewModel.cs

Views/
    Account/
    Clientes/
    Dashboard/
    Reportes/
    Shared/
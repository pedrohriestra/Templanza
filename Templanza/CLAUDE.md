# Templanza — Contexto del proyecto

Tienda online de hierbas para té con foro comunitario de "blends" (combinaciones de hierbas armadas por los usuarios). Trabajo final de la materia de Desarrollo Web.

## Stack técnico

- .NET 9, ASP.NET Core MVC
- Entity Framework Core, **Code First** con migraciones
- SQL Server local (o SQLite si no hay SQL Server disponible)
- ASP.NET Core Identity para autenticación y roles
- Inyección de dependencias configurada en `Program.cs`
- Bootstrap + DataTables + SweetAlert/Toastr en el frontend
- ViewModels para separar dominio de presentación
- Al menos un listado resuelto con Stored Procedure (reporte de ventas por rango de fechas)

## Idea de negocio

Catálogo de **Plantas** (hierbas individuales: manzanilla, menta, jengibre, etc.) que se compran directamente. En paralelo, un foro donde los usuarios arman **Blends**: combinaciones de esas plantas a modo de receta/inspiración, sin compra directa desde el blend.

## Modelo de dominio

**Planta** — producto de la tienda
- Nombre común, nombre científico, efectos, contraindicaciones, parte usada, origen
- Precio, Stock
- Relación con Categoria

**Blend** — posteo del foro (NO es un producto)
- Nombre, descripción
- Receta armada vía `BlendPlanta` (N:N con Planta, con proporción/cantidad)
- `EsPublicado`: requiere aprobación de Admin/Operador antes de ser visible (moderación)
- `EsRecomendado`: distingue dos orígenes en la misma tabla
  - `false` → foro comunitario: lo crea cualquier usuario, pasa por moderación, tiene comentarios y likes
  - `true` → recomendados: recetas oficiales curadas por el Admin, se auto-publican, son solo vitrina (sin comentarios ni likes)

**Interacción social**
- `Comentario`: comentarios de usuarios sobre un blend publicado
- `BlendLike`: relación N:N usuario-blend ("me gusta")

**Resto de entidades**
- `Categoria`: clasifica plantas y blends
- `Efecto` + `PlantaEfecto`: catálogo de efectos (relajante, digestivo, etc.) con intensidad, N:N con Planta
- `Orden` + `ItemOrden`: carrito/compra de Plantas, con precio congelado al momento de la compra
- `CorreoEnviado`: log de notificaciones por email
- `ApplicationUser`: extiende Identity con `Nombre` e `ImagenUrl`

## Roles y accesos

Tres roles vía Identity: **Administrador**, **Operador**, **Cliente**.
- Backoffice con `[Authorize(Roles = "...")]` y `[Area("Admin")]`
- Ahí se gestiona el catálogo, se aprueban/rechazan blends del foro (`Blends/Pendientes`) y se cargan blends recomendados (`Blends/CreateRecomendado`)

## Requisitos obligatorios de la cátedra (resumen)

**Configuración inicial**
- .NET 8+ (usamos .NET 9), ASP.NET Core MVC
- EF Core Code First con migraciones
- SQL Server (mín. 6 tablas) o SQLite
- DI configurada, herramientas front-end tipo SweetAlert

**Modelo de dominio**
- Mínimo 6 entidades principales
- Propiedades obligatorias y opcionales
- Relaciones 1:N, N:N o 1:1
- Data Annotations (`[Required]`, `[StringLength]`, `[Range]`, etc.)

**Capa de datos**
- `DbContext` configurado en `Program.cs`
- Mínimo 4 migraciones (inicial + creación modelos + cambios posteriores)
- Data Seeding con datos iniciales

**Funcionalidad MVC**
- CRUD completo para mínimo 6 entidades, vistas Razor fuertemente tipadas
- ViewModels
- Tag Helpers (`asp-for`, `asp-action`, `asp-route`, etc.)
- Validación en cliente y servidor
- Dropdowns anidados con al menos 2 modelos
- Un listado usando Stored Procedures

**UX**
- Layout compartido (`_Layout.cshtml`) y menú de navegación
- Al menos una búsqueda/filtro sensitivo hecho a mano; el resto con DataTable
- Al menos una paginación/sorting hecho a mano; el resto con DataTable

**Seguridad**
- ASP.NET Core Identity completo (registro, login, recuperación, perfiles)
- Roles: Administrador / Operador / Cliente
- Control de acceso con `[Authorize(Roles=...)]`, `[Area("Admin")]`, y checks tipo `RoleManager.RoleExistsAsync`

**Extras opcionales (no obligatorios)**
- API REST complementaria
- Subida/gestión de archivos (imágenes/PDF)
- Partial Views / View Components
- Dockerizar
- AdminLTE

**Entregables**
- Repo GitHub/GitLab con commits claros
- `README.md` con descripción, instrucciones de instalación/ejecución y capturas de pantalla

**Nota importante:** el trabajo es de hasta 2 integrantes pero la nota es individual y condicionada — si uno desaprueba, desaprueban ambos.

## Convenciones del proyecto

- Nombres de entidades, propiedades y vistas en español, consistentes con el dominio (Planta, Blend, Orden, etc.)
- Uso de Areas: al menos `Admin` para el backoffice
- Repository pattern / unidad de trabajo si el tiempo lo permite (visto en la unidad 3 de la materia)
- Priorizar que cada entidad tenga su CRUD Razor fuertemente tipado antes de sumar extras opcionales

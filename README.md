# Templanza

Trabajo final de la materia de Desarrollo Web. Tienda online de hierbas para té, con un foro comunitario de **blends** (combinaciones de hierbas armadas y compartidas por los usuarios).

🔗 **Demo en vivo:** [templanza.onrender.com](https://templanza.onrender.com)

### Credenciales de acceso (demo en vivo)

| Rol | Email | Contraseña |
|---|---|---|
| Administrador | `admin@templanza.com` | `Admin123!` |

> El plan free de Render duerme el servicio tras un rato sin tráfico; si la primera carga tarda unos segundos en responder, es normal.

## Descripción del proyecto

Templanza combina dos cosas en una sola app:

- **Tienda**: catálogo de plantas (hierbas individuales) que se compran directamente, con carrito, orden con precio congelado y pago con Mercado Pago (QR/redirect según el dispositivo).
- **Foro de blends**: cualquier usuario registrado puede armar un *blend* (receta con varias plantas y sus proporciones) y publicarlo. Pasa por moderación de un Admin/Operador antes de ser visible, y admite comentarios y "me gusta". Aparte existen los **blends recomendados**: recetas oficiales curadas por Templanza, que se publican directo sin pasar por moderación.

### Stack técnico

- .NET 9, ASP.NET Core MVC
- Entity Framework Core (Code First) con migraciones, SQL Server (LocalDB en desarrollo)
- ASP.NET Core Identity (registro, login, recuperación de contraseña, roles, perfil)
- Envío real de emails (confirmación de cuenta y recuperación de contraseña) vía Gmail SMTP con MailKit
- Bootstrap 5 + DataTables + SweetAlert2
- Reporte de ventas resuelto con un stored procedure
- Dockerizado, desplegado en Render (app) + Somee (SQL Server)

### Modelo de dominio

`Categoria`, `Planta`, `Efecto` (N:N vía `PlantaEfecto` con intensidad), `Blend`, `BlendPlanta` (N:N con cantidad/unidad), `Comentario`, `BlendLike`, `Orden`/`ItemOrden` (con precio congelado al momento de compra), `CorreoEnviado` (log de emails), además de `ApplicationUser` (extiende Identity con Nombre e ImagenUrl).

### Roles

- **Administrador**: acceso total al backoffice (`/Admin`). Además de todo lo que puede hacer el Operador, es el único que puede crear categorías/efectos/plantas/blends recomendados, gestionar usuarios (alta, roles, confirmación de email) y ver el log de correos enviados.
- **Operador**: acceso al backoffice para el día a día: puede ver, editar y eliminar Categorías, Efectos, Plantas y Blends recomendados, moderar el foro (aprobar/rechazar blends pendientes), moderar Comentarios, y ver Órdenes y el Reporte de ventas. No puede crear plantas/categorías nuevas ni gestionar usuarios.
- **Cliente**: rol asignado automáticamente al registrarse. Compra en la tienda y participa del foro.

## Despliegue

La demo en vivo corre containerizada:

- **Base de datos**: SQL Server gratuito en [Somee](https://somee.com), con las migraciones y el data seeding aplicados.
- **Aplicación**: [Render](https://render.com) como Web Service, buildeado a partir del `Dockerfile` de la raíz del repo (multi-stage: SDK de .NET 9 para compilar, runtime ASP.NET para correr). Al arrancar, la propia app aplica las migraciones pendientes contra la base (`Database.MigrateAsync()`) y siembra roles + usuario administrador si todavía no existen — no hace falta correr nada a mano en el servidor.

Variables de entorno configuradas en Render (no viven en el repo, se cargan desde el panel de Render):

| Variable | Contenido |
|---|---|
| `ConnectionStrings__DefaultConnection` | Connection string de la base en Somee |
| `Smtp__Usuario` | Email de Gmail usado para enviar correos |
| `Smtp__Password` | Contraseña de aplicación de ese Gmail |

> Nota: el plan free de Render duerme el servicio tras un rato sin tráfico y tarda unos segundos en despertar en el próximo request — es normal si la primera carga tarda un poco.

## Instalación y ejecución

### Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (viene con Visual Studio) o una instancia de SQL Server accesible
- Una cuenta de Gmail con una [contraseña de aplicación](https://myaccount.google.com/apppasswords) si querés que los emails de confirmación/recuperación se envíen de verdad (opcional para levantar el proyecto)

### Pasos

1. Cloná el repositorio y abrí la carpeta `Templanza` (donde está el `.csproj`):

   ```bash
   git clone https://github.com/pedrohriestra/Templanza.git
   cd Templanza/Templanza
   ```

2. Configurá la cadena de conexión en `appsettings.json` si no usás LocalDB con el nombre por default (ya viene lista para LocalDB, no hace falta tocar nada en la mayoría de los casos).

3. (Opcional) Configurá el envío de emails con `dotnet user-secrets` — si no lo hacés, el registro/login van a funcionar igual, pero no va a llegar ningún email real:

   ```bash
   dotnet user-secrets set "Smtp:Usuario" "tu-email@gmail.com"
   dotnet user-secrets set "Smtp:Password" "tu-contraseña-de-aplicacion-de-16-caracteres"
   ```

4. Aplicá las migraciones (crea la base de datos con el esquema y los datos iniciales):

   ```bash
   dotnet ef database update
   ```

5. Corré la aplicación:

   ```bash
   dotnet run
   ```

6. Abrí `https://localhost:{puerto}` (el puerto que indique la consola). Al arrancar por primera vez, la app crea sola los roles (Administrador, Operador, Cliente) y un usuario administrador:

   ```
   Email: admin@templanza.com
   Contraseña: Admin123!
   ```

### Pago con Mercado Pago

El link de cobro se configura en `appsettings.json` → `MercadoPago:PaymentLink`. Al confirmar una compra, la orden queda en estado *Pendiente*; el pago se verifica manualmente contra la cuenta de Mercado Pago y se confirma desde `Admin > Órdenes`.

## Funcionalidades principales

### Tienda pública

- **Inicio**: portada del sitio.
- **Catálogo** (filtro por categoría): listado de plantas con botones para filtrar por categoría.
- **Detalle de planta**: ficha con efectos asociados y stepper para agregar al carrito.

### Foro de blends

- **Foro comunitario**: blends publicados y aprobados por moderación.
- **Detalle de blend**: receta completa, comentarios y likes.
- **Blends recomendados**: vitrina de recetas oficiales curadas por Templanza.

### Cuenta

- **Registro**: alta de usuario con envío real de email de confirmación.
- **Login / recuperación de contraseña**: vía ASP.NET Core Identity.

### Compra y pago

- **Orden pendiente con QR de Mercado Pago**: al confirmar la compra, la orden queda pendiente hasta verificar el pago (en mobile se muestra un botón de redirect en vez del QR).

### Panel de administración (`/Admin`)

- **Dashboard**: accesos rápidos según el rol logueado.
- **CRUD de Plantas**: búsqueda y paginación hechas a mano.
- **CRUD de Categorías** y **Efectos**: listados con DataTable.
- **Blends recomendados**: alta/edición/baja de las recetas oficiales.
- **Moderación de blends pendientes**: aprobar o rechazar lo que manda la comunidad.
- **Comentarios**: moderación de comentarios del foro.
- **Órdenes**: ver compras y cambiar su estado.
- **Reporte de ventas**: stored procedure, por rango de fechas.
- **Correos enviados**: log de auditoría de los emails, con estado de éxito/error.
- **Usuarios**: alta, baja y edición de cuentas — nombre, email (con confirmación manual), teléfono, contraseña y rol.

## Integrantes

Pedro Herrera Riestra

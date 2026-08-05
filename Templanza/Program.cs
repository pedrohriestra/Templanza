using System.Globalization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;
using Templanza.Models;
using Templanza.Services;

var builder = WebApplication.CreateBuilder(args);

// Puerto asignado dinámicamente por el hosting (Render).
var puertoAsignado = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(puertoAsignado))
{
    builder.WebHost.UseUrls($"http://+:{puertoAsignado}");
}

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IEmailSender, EmailSenderGmail>();
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, IdentityUiEmailSenderAdapter>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Reconoce el esquema/IP originales detrás del proxy de Render.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var culturaApp = new[] { new CultureInfo("es-AR") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es-AR"),
    SupportedCultures = culturaApp,
    SupportedUICultures = culturaApp
});

app.UseHttpsRedirection();
app.UseRouting();

// La app no usa doble factor de autenticación: estas páginas siguen embebidas en el
// paquete de Identity UI aunque no las scaffoleamos ni las linkeamos, así que se bloquean
// explícitamente para que no queden accesibles por URL directa.
string[] rutas2faBloqueadas =
[
    "/Identity/Account/LoginWith2fa",
    "/Identity/Account/LoginWithRecoveryCode",
    "/Identity/Account/Manage/TwoFactorAuthentication",
    "/Identity/Account/Manage/EnableAuthenticator",
    "/Identity/Account/Manage/Disable2fa",
    "/Identity/Account/Manage/GenerateRecoveryCodes",
    "/Identity/Account/Manage/ResetAuthenticator",
    "/Identity/Account/Manage/ShowRecoveryCodes",
];

app.Use(async (context, next) =>
{
    var esRuta2fa = rutas2faBloqueadas.Any(ruta =>
        context.Request.Path.StartsWithSegments(ruta, StringComparison.OrdinalIgnoreCase));

    if (esRuta2fa)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return;
    }

    await next();
});

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

// Aplica migraciones pendientes y siembra roles/admin al arrancar.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    await SeedRolesYAdminAsync(scope.ServiceProvider);
}

app.Run();

// Crea los 3 roles y un usuario Administrador si todavía no existen.
static async Task SeedRolesYAdminAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { Roles.Administrador, Roles.Operador, Roles.Cliente };
    foreach (var rol in roles)
    {
        if (!await roleManager.RoleExistsAsync(rol))
        {
            await roleManager.CreateAsync(new IdentityRole(rol));
        }
    }

    const string adminEmail = "admin@templanza.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser is null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            Nombre = "Administrador",
            EmailConfirmed = true
        };

        var resultado = await userManager.CreateAsync(adminUser, "Admin123!");
        if (resultado.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, Roles.Administrador);
        }
    }
}

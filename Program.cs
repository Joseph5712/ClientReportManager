using ClientReportManager.Data;
using ClientReportManager.Models;
using ClientReportManager.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Se habilita MVC para trabajar con controladores y vistas Razor.
builder.Services.AddControllersWithViews();

// Se registra el DbContext usando SQL Server.
// La cadena de conexión se toma desde appsettings.json.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Se configura autenticación por cookies para proteger las pantallas internas del sistema.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// Se registra PasswordHasher para validar contraseñas de forma segura.
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

// Se registra el servicio de clientes para separar la lógica del controlador.
builder.Services.AddScoped<IClienteService, ClienteService>();

// Se registra el servicio del dashboard para centralizar las consultas de resumen.
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Se registra el servicio de reportes para centralizar filtros, resúmenes y consultas administrativas.
builder.Services.AddScoped<IReporteService, ReporteService>();

// Servicio de usuarios utilizado por el proceso de login.
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

var app = builder.Build();

// Configuración de errores para ambientes diferentes a desarrollo.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware base de la aplicación web.
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// La autenticación debe ejecutarse antes de la autorización.
app.UseAuthentication();
app.UseAuthorization();

// Ruta principal del sistema.
// En esta etapa, el Dashboard estará protegido y solicitará login si el usuario no está autenticado.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
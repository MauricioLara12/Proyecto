using Microsoft.AspNetCore.Authentication.Cookies;
using CCTP_Manage.Models.Entities;
using CCTP_Manage.Models.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configurar autenticaci�n basada en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

// Agregar soporte para MVC
builder.Services.AddMvc();

// Agrega servicios al contenedor
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<SaloneventosdbContext>();

// Registra el servicio en segundo plano
builder.Services.AddHostedService<EmailReminderService>();

var app = builder.Build();

// Configurar el middleware
app.UseAuthentication();  // Middleware de autenticaci�n
app.UseAuthorization();   // Middleware de autorizaci�n
app.UseStaticFiles();      // Habilitar archivos est�ticos

// Configurar rutas predeterminadas
app.MapDefaultControllerRoute(); // Ruta {controller=Home}/{action=Index}/{id?}

// Iniciar la aplicaci�n
app.Run();

using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configurar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddMvc();

// Registrar el controlador de reservaciones
builder.Services.AddScoped<ReservacionesController>();

// Registrar EmailHelper
builder.Services.AddScoped<EmailHelper>();

// Registrar el servicio de fondo
builder.Services.AddHostedService<ReservacionesBackgroundService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapDefaultControllerRoute();

app.Run();

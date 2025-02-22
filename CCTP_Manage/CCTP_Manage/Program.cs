using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication
    (CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddMvc();
var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.UseStaticFiles();

app.Run();
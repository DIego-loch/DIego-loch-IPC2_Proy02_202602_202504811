var builder = WebApplication.CreateBuilder(args);

// Servicios de MVC (controladores + vistas)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Archivos estáticos (CSS, JS, imágenes)
app.UseStaticFiles();

// Rutas
app.UseRouting();

// Ruta por defecto: Home/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

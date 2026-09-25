using System.IO;
using Proyecto2.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Controladores
builder.Services.AddControllers();

// Sistema singleton (mantiene el estado entre requests)
builder.Services.AddSingleton<SistemaLibreria>();
builder.Services.AddSingleton<CargadorXML>();

// Generador Graphviz → guarda los PNG en wwwroot/Reportes
builder.Services.AddSingleton<GeneradorGraphviz>(sp =>
    new GeneradorGraphviz(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "Reportes")));

// CORS para desarrollo
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseCors();

// Sirve index.html por defecto desde wwwroot
app.UseDefaultFiles();

// Sirve css, js, imágenes y la carpeta Reportes
app.UseStaticFiles();

app.MapControllers();

app.Run();

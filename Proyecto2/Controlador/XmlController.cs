using Microsoft.AspNetCore.Mvc;
using Servicios;
using ArbolCategoria;

namespace Proyecto2.Controlador;

public class XmlController : Controller
{
    private static Arbol_Categoria _arbol = new Arbol_Categoria();
    private static CargadorXML _cargador = new CargadorXML();

    public static Arbol_Categoria ObtenerArbol() => _arbol;

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Cargar(IFormFile archivo)
    {
        if (archivo == null || archivo.Length == 0)
        {
            ViewBag.Error = "No se subió ningún archivo.";
            return View("Index");
        }

        try
        {
            using var stream = archivo.OpenReadStream();
            bool ok = _cargador.CargarDesdeStream(stream, _arbol);

            if (!ok)
            {
                ViewBag.Error = "Error al procesar el XML.";
                return View("Index");
            }
        }
        catch (System.Exception e)
        {
            ViewBag.Error = "Error: " + e.Message;
            return View("Index");
        }

        return RedirectToAction("Index", "Catalogo");
    }
}

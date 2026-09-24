using Microsoft.AspNetCore.Mvc;
using ArbolCategoria;

namespace Proyecto2.Controlador;

public class CatalogoController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var arbol = XmlController.ObtenerArbol();
        return View(arbol);
    }

    [HttpPost]
    public IActionResult AgregarCategoria(string nombre, string padre)
    {
        var arbol = XmlController.ObtenerArbol();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            TempData["Error"] = "El nombre no puede estar vacío.";
            return RedirectToAction("Index");
        }

        if (arbol.EstaVacio())
        {
            arbol.InsertarRaiz(nombre);
        }
        else if (arbol.Existe(nombre))
        {
            TempData["Error"] = "Ya existe una categoría con ese nombre.";
        }
        else if (string.IsNullOrWhiteSpace(padre))
        {
            var raiz = arbol.raiz;
            if (raiz != null)
                arbol.Insertar(nombre, raiz.Nodo_actual.nombre_categoria);
        }
        else
        {
            arbol.Insertar(nombre, padre);
        }

        return RedirectToAction("Index");
    }
}

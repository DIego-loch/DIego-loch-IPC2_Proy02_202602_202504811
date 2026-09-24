using Microsoft.AspNetCore.Mvc;
using Clase_libro;

namespace Proyecto2.Controlador;

public class LibroController : Controller
{
    [HttpPost]
    public IActionResult Agregar(int isbn, string titulo, string autor, string categoria)
    {
        var arbol = XmlController.ObtenerArbol();
        var nodo = arbol.Buscar(categoria);

        if (nodo == null)
        {
            TempData["Error"] = "Categoría no encontrada: " + categoria;
            return RedirectToAction("Index", "Catalogo");
        }

        if (nodo.Nodo_actual.lista_libros!.BuscarInfo(isbn) != null)
        {
            TempData["Error"] = "Ya existe un libro con ISBN " + isbn;
            return RedirectToAction("Index", "Catalogo");
        }

        Libro libro = new Libro(isbn, titulo, autor, nodo.Nodo_actual);
        nodo.Nodo_actual.lista_libros.IngresarLibro(libro);

        return RedirectToAction("Index", "Catalogo");
    }

    [HttpPost]
    public IActionResult Eliminar(int isbn, string categoria)
    {
        var arbol = XmlController.ObtenerArbol();
        var nodo = arbol.Buscar(categoria);

        if (nodo == null)
        {
            TempData["Error"] = "Categoría no encontrada.";
            return RedirectToAction("Index", "Catalogo");
        }

        bool ok = nodo.Nodo_actual.lista_libros!.Eliminar(isbn);
        if (!ok) TempData["Error"] = "No se encontró el ISBN " + isbn;

        return RedirectToAction("Index", "Catalogo");
    }

    [HttpGet]
    public IActionResult Buscar(int isbn, string categoria)
    {
        var arbol = XmlController.ObtenerArbol();
        var nodo = arbol.Buscar(categoria);
        if (nodo == null) return NotFound();

        var encontrado = nodo.Nodo_actual.lista_libros!.BuscarInfo(isbn);
        if (encontrado == null) return NotFound();

        return Json(new
        {
            ISBN = encontrado.Nodo_actual.ISBN,
            Titulo = encontrado.Nodo_actual.titulo,
            Autor = encontrado.Nodo_actual.autor
        });
    }

    [HttpGet]
    public IActionResult Minimo(string categoria)
    {
        var arbol = XmlController.ObtenerArbol();
        var nodo = arbol.Buscar(categoria);
        if (nodo == null) return NotFound();

        var min = nodo.Nodo_actual.lista_libros!.Min();
        if (min == null) return NotFound();
        return Json(new { ISBN = min.ISBN, Titulo = min.titulo, Autor = min.autor });
    }

    [HttpGet]
    public IActionResult Maximo(string categoria)
    {
        var arbol = XmlController.ObtenerArbol();
        var nodo = arbol.Buscar(categoria);
        if (nodo == null) return NotFound();

        var max = nodo.Nodo_actual.lista_libros!.Max();
        if (max == null) return NotFound();
        return Json(new { ISBN = max.ISBN, Titulo = max.titulo, Autor = max.autor });
    }
}

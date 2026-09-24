using Microsoft.AspNetCore.Mvc;

namespace Proyecto2.Controlador;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Content("Servidor MVC funcionando correctamente");
    }
}

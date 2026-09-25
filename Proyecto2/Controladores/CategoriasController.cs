using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Proyecto2.Modelo;
using Proyecto2.Servicios;

namespace Proyecto2.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly SistemaLibreria _sistema;
        private readonly GeneradorGraphviz _gv;

        public CategoriasController(SistemaLibreria sistema, GeneradorGraphviz gv)
        {
            _sistema = sistema;
            _gv = gv;
        }

        // GET /api/categorias?desde=Ficción
        [HttpGet]
        public IActionResult Get([FromQuery] string desde)
        {
            ListaCadenas lista = _sistema.MostrarEstructura(desde);

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            NodoCadena a = lista.Raiz;
            bool primero = true;
            while (a != null)
            {
                if (!primero) sb.Append(",");
                primero = false;

                sb.Append("{\"nombre\":\"").Append(EscaparJSON(a.Valor)).Append("\",");
                sb.Append("\"nivel\":").Append(a.Nivel).Append("}");

                a = a.Siguiente;
            }
            sb.Append("]");

            return Content(sb.ToString(), "application/json");
        }

        // POST /api/categorias
        [HttpPost]
        public IActionResult Post([FromBody] CategoriaDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Nombre))
                return Content("{\"ok\":false,\"msg\":\"Nombre vacío\"}", "application/json");

            bool ok = _sistema.AgregarCategoria(dto.Nombre, dto.Padre);
            if (!ok)
                return Content("{\"ok\":false,\"msg\":\"Duplicado o padre inexistente\"}",
                               "application/json");

            return Content("{\"ok\":true}", "application/json");
        }

        // GET /api/categorias/grafo  → genera PNG del árbol de categorías
        [HttpGet("grafo")]
        public IActionResult Grafo()
        {
            string img = _gv.GenerarArbolCategorias(_sistema.Categorias);
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"imagen\":\"").Append(EscaparJSON(img)).Append("\"}");
            return Content(sb.ToString(), "application/json");
        }

        private string EscaparJSON(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .Replace("\t", "\\t");
        }
    }

    public class CategoriaDTO
    {
        public string Nombre { get; set; }
        public string Padre { get; set; }
    }
}

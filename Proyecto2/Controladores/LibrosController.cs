using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Proyecto2.Modelo;
using Proyecto2.Servicios;

namespace Proyecto2.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly SistemaLibreria _sistema;
        private readonly GeneradorGraphviz _gv;

        public LibrosController(SistemaLibreria sistema, GeneradorGraphviz gv)
        {
            _sistema = sistema;
            _gv = gv;
        }


        [HttpGet]
        public IActionResult Get()
        {
            ListaLibros lista = _sistema.LibrosAscendente();
            string json = ListaLibrosAJson(lista, true);
            return Content(json, "application/json");
        }


        [HttpGet("{isbn}")]
        public IActionResult Get(int isbn)
        {
            Libro l = _sistema.BuscarLibro(isbn);
            if (l == null)
                return Content("{\"error\":\"No encontrado\"}", "application/json");

            return Content(LibroAJson(l), "application/json");
        }


        [HttpPost]
        public IActionResult Post([FromBody] LibroDTO dto)
        {
            if (dto == null)
                return BadRequest();

            bool ok = _sistema.AgregarLibro(dto.ISBN, dto.Titulo, dto.Autor, dto.Categoria);
            if (!ok)
                return Content("{\"ok\":false,\"msg\":\"No se pudo agregar (ISBN duplicado o categoría inexistente)\"}",
                               "application/json");

            return Content("{\"ok\":true}", "application/json");
        }


        [HttpDelete("{isbn}")]
        public IActionResult Delete(int isbn)
        {
            if (_sistema.EliminarLibro(isbn))
                return Content("{\"ok\":true}", "application/json");
            return Content("{\"ok\":false,\"msg\":\"No existe\"}", "application/json");
        }


        [HttpGet("menor")]
        public IActionResult Menor()
        {
            Libro l = _sistema.LibroMenor();
            if (l == null)
                return Content("{\"error\":\"Catálogo vacío\"}", "application/json");
            return Content(LibroAJson(l), "application/json");
        }


        [HttpGet("mayor")]
        public IActionResult Mayor()
        {
            Libro l = _sistema.LibroMayor();
            if (l == null)
                return Content("{\"error\":\"Catálogo vacío\"}", "application/json");
            return Content(LibroAJson(l), "application/json");
        }


        [HttpGet("categoria/{nombre}")]
        public IActionResult PorCategoria(string nombre)
        {
            ListaLibros lista = _sistema.LibrosDeCategoria(nombre);
            Categoria cat = _sistema.BuscarCategoria(nombre);

            string img = "";
            if (cat != null)
                img = _gv.GenerarArbolLibros(cat.Libros, nombre);

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"libros\":");
            sb.Append(ListaLibrosAJson(lista, true));
            sb.Append(",\"imagen\":\"").Append(EscaparJSON(img)).Append("\"}");

            return Content(sb.ToString(), "application/json");
        }



        private string LibroAJson(Libro l)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"isbn\":").Append(l.ISBN).Append(",");
            sb.Append("\"titulo\":\"").Append(EscaparJSON(l.Titulo)).Append("\",");
            sb.Append("\"autor\":\"").Append(EscaparJSON(l.Autor)).Append("\",");
            sb.Append("\"categoria\":\"");
            if (l.Categoria != null) sb.Append(EscaparJSON(l.Categoria.Nombre));
            sb.Append("\"");
            sb.Append("}");
            return sb.ToString();
        }


        private string ListaLibrosAJson(ListaLibros lista, bool comoArreglo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            NodoListaLibro a = lista.Raiz;
            bool primero = true;
            while (a != null)
            {
                if (!primero) sb.Append(",");
                primero = false;
                sb.Append(LibroAJson(a.Dato));
                a = a.Siguiente;
            }
            sb.Append("]");
            return sb.ToString();
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


    public class LibroDTO
    {
        public int ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }
    }
}

using System;

namespace Proyecto2.Modelo
{
    public class Categoria
    {
        public string Nombre { get; set; }
        public ListaHijos Hijos { get; set; }
        public ArbolLibros Libros { get; set; }

        public Categoria(string nombre)
        {
            Nombre = nombre;
            Hijos = new ListaHijos();
            Libros = new ArbolLibros();
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}

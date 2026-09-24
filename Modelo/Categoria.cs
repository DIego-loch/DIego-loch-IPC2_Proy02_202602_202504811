using System;
using List_enlazada;
using ArbolLibro;

namespace Categoria_libro;

public class Categoria
{
    public string nombre_categoria { get; set; }
    public Lista_hijos? lista_hijos { get; set; }
    public Arbol_libro? lista_libros { get; set; }

    public Categoria(string nombre)
    {
        nombre_categoria = nombre;
        lista_hijos = new Lista_hijos();
        lista_libros = new Arbol_libro();
    }
}

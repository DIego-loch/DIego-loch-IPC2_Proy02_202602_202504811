using System;
using Categoria_libro;
using List_enlazada;

namespace Nodo_Categoria;

public class Nodo_categoria
{
    public Categoria Nodo_actual { get; set; }
    public Lista_hijos hijos { get; set; }
    public Nodo_categoria? siguiente { get; set; }

    public Nodo_categoria(Categoria dato)
    {
        Nodo_actual = dato;
        hijos = new Lista_hijos();
        siguiente = null;
    }
}

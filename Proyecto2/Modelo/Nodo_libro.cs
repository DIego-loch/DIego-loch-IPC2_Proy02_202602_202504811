using System;
using Clase_libro;
namespace Nodo_libros;

public class Nodo_libro
{
    public Libro Nodo_actual{ get; set; }
    public Nodo_libro? Nodo_izquierda { get; set; }
    public Nodo_libro? Nodo_derecha { get; set; }

    public Nodo_libro(Libro dato)
    {
        Nodo_actual = dato;
        Nodo_izquierda = null;
        Nodo_derecha = null;
    }
}

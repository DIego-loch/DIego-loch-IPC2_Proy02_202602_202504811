using System;
using Clase_libro;
using Nodo_libros;

namespace ArbolLibro;

public class Arbol_libro
{
    public Nodo_libro? raiz { get; set; }
    public Nodo_libro? nodo_anterior { get; set; }

    public void IngresarLibro(Libro libro)
    {
        Nodo_libro nodo = new Nodo_libro(libro);

        if (raiz == null)
        {
            raiz = nodo;
            return;

        }
        Nodo_libro padre = raiz;
        while (true)
        {
            if (nodo.Nodo_actual.ISBN < padre.Nodo_actual.ISBN)
            {
                if (padre.Nodo_izquierda == null)
                {
                    padre.Nodo_izquierda = nodo;
                    return;
                }
                else
                {
                    padre = padre.Nodo_izquierda;
                }
            }
            else if (nodo.Nodo_actual.ISBN > padre.Nodo_actual.ISBN)
            {
                if (padre.Nodo_derecha == null)
                {
                    padre.Nodo_derecha = nodo;
                    return;
                }
                else
                {
                    padre = padre.Nodo_derecha;
                }
            }
            else
            {
                return;
            }
        }

    }
    public Nodo_libro BuscarInfo(int isbn)
    {
        Nodo_libro buscador = raiz;
        while (buscador != null)
        {
            if (isbn == buscador.Nodo_actual.ISBN)
            {
                return buscador;
            }
            else if (isbn < buscador.Nodo_actual.ISBN)
            {
                nodo_anterior = buscador;
                buscador = buscador.Nodo_izquierda;
            }
            else if (isbn > buscador.Nodo_actual.ISBN)
            {
                nodo_anterior = buscador;
                buscador = buscador.Nodo_derecha;
            }
        }
        return null;
    }

    public Libro Min()
    {
        Nodo_libro min = raiz;
        if (min == null) return null;
        while (min.Nodo_izquierda != null)
        {
            min = min.Nodo_izquierda;
        }
        return min.Nodo_actual;
    }

    public Libro Max()
    {
        Nodo_libro max = raiz;
        if (max == null) return null;
        while (max.Nodo_derecha != null)
        {
            max = max.Nodo_derecha;
        }
        return max.Nodo_actual;
    }

    public void Recorrer()
    {
        Nodo_libro inorden = raiz;
        Inorden(inorden);
        return;
    }
    public void Inorden(Nodo_libro inorden)
    {
        if (inorden == null) return;
        Inorden(inorden.Nodo_izquierdo);
        Console.WriteLine(inorden.Nodo_actual.ISBN);
        Inorden(inorden.Nodo_derecho);

    }

    public bool Eliminar(int isbn)
    {
        if (raiz == null) return false;

        // Verificar que exista antes de eliminar
        if (BuscarInfo(isbn) == null) return false;

        raiz = EliminarRecursivo(raiz, isbn);
        return true;
    }

    private Nodo_libro? EliminarRecursivo(Nodo_libro? nodo, int isbn)
    {
        if (nodo == null) return null;

        if (isbn < nodo.Nodo_actual.ISBN)
        {
            nodo.Nodo_izquierda = EliminarRecursivo(nodo.Nodo_izquierda, isbn);
        }
        else if (isbn > nodo.Nodo_actual.ISBN)
        {
            nodo.Nodo_derecha = EliminarRecursivo(nodo.Nodo_derecha, isbn);
        }
        else
        {
            // Caso 1 y 2: 0 hijos o 1 hijo
            if (nodo.Nodo_izquierda == null)
                return nodo.Nodo_derecha;
            if (nodo.Nodo_derecha == null)
                return nodo.Nodo_izquierda;

            // Caso 3: 2 hijos → buscar sucesor inorden
            Nodo_libro sucesor = MinimoNodo(nodo.Nodo_derecha);

            // Copiar los datos del sucesor al nodo actual
            nodo.Nodo_actual = sucesor.Nodo_actual;

            // Eliminar el sucesor original
            nodo.Nodo_derecha = EliminarRecursivo(nodo.Nodo_derecha, sucesor.Nodo_actual.ISBN);
        }

        return nodo;
    }

    private Nodo_libro MinimoNodo(Nodo_libro nodo)
    {
        while (nodo.Nodo_izquierda != null)
        {
            nodo = nodo.Nodo_izquierda;
        }
        return nodo;
    }

}

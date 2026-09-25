using System;

namespace Proyecto2.Modelo
{
    public class NodoListaLibro
    {
        public Libro Dato { get; set; }
        public NodoListaLibro Siguiente { get; set; }

        public NodoListaLibro(Libro dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }

    // Lista enlazada simple de libros (para resultados, reportes, etc.)
    public class ListaLibros
    {
        public NodoListaLibro Raiz { get; set; }
        public int Cantidad { get; set; }

        public ListaLibros()
        {
            Raiz = null;
            Cantidad = 0;
        }

        public bool EstaVacia()
        {
            return Raiz == null;
        }

        // Inserta al final
        public void Agregar(Libro libro)
        {
            NodoListaLibro nuevo = new NodoListaLibro(libro);

            if (Raiz == null)
            {
                Raiz = nuevo;
                Cantidad = 1;
                return;
            }

            NodoListaLibro actual = Raiz;
            while (actual.Siguiente != null)
                actual = actual.Siguiente;

            actual.Siguiente = nuevo;
            Cantidad++;
        }
    }
}

using System;

namespace IPC2_Proy02.Modelo
{
    public class ArbolLibros
    {
        public NodoLibro Raiz { get; set; }

        public ArbolLibros()
        {
            Raiz = null;
        }

        public bool EstaVacio()
        {
            return Raiz == null;
        }

        public bool Insertar(Libro libro)
        {
            NodoLibro nuevo = new NodoLibro(libro);

            if (Raiz == null)
            {
                Raiz = nuevo;
                return true;
            }

            NodoLibro actual = Raiz;
            while (true)
            {
                if (libro.ISBN < actual.Dato.ISBN)
                {
                    if (actual.Izquierda == null)
                    {
                        actual.Izquierda = nuevo;
                        return true;
                    }
                    actual = actual.Izquierda;
                }
                else if (libro.ISBN > actual.Dato.ISBN)
                {
                    if (actual.Derecha == null)
                    {
                        actual.Derecha = nuevo;
                        return true;
                    }
                    actual = actual.Derecha;
                }
                else
                {
                    return false; // ISBN duplicado
                }
            }
        }

        public Libro Buscar(int isbn)
        {
            NodoLibro actual = Raiz;
            while (actual != null)
            {
                if (isbn == actual.Dato.ISBN) return actual.Dato;
                actual = isbn < actual.Dato.ISBN
                    ? actual.Izquierda
                    : actual.Derecha;
            }
            return null;
        }

        public Libro Minimo()
        {
            if (Raiz == null) return null;
            NodoLibro a = Raiz;
            while (a.Izquierda != null) a = a.Izquierda;
            return a.Dato;
        }

        public Libro Maximo()
        {
            if (Raiz == null) return null;
            NodoLibro a = Raiz;
            while (a.Derecha != null) a = a.Derecha;
            return a.Dato;
        }

        public bool Eliminar(int isbn)
        {
            if (Buscar(isbn) == null) return false;
            Raiz = EliminarRec(Raiz, isbn);
            return true;
        }

        private NodoLibro EliminarRec(NodoLibro nodo, int isbn)
        {
            if (nodo == null) return null;

            if (isbn < nodo.Dato.ISBN)
                nodo.Izquierda = EliminarRec(nodo.Izquierda, isbn);
            else if (isbn > nodo.Dato.ISBN)
                nodo.Derecha = EliminarRec(nodo.Derecha, isbn);
            else
            {
                if (nodo.Izquierda == null) return nodo.Derecha;
                if (nodo.Derecha == null) return nodo.Izquierda;

                NodoLibro sucesor = MinimoNodo(nodo.Derecha);
                nodo.Dato = sucesor.Dato;
                nodo.Derecha = EliminarRec(nodo.Derecha, sucesor.Dato.ISBN);
            }
            return nodo;
        }

        private NodoLibro MinimoNodo(NodoLibro n)
        {
            while (n.Izquierda != null) n = n.Izquierda;
            return n;
        }

        // ✅ Devuelve TDA propio ListaLibros (sin arreglos)
        public ListaLibros Inorden()
        {
            ListaLibros lista = new ListaLibros();
            InordenRec(Raiz, lista);
            return lista;
        }

        private void InordenRec(NodoLibro n, ListaLibros lista)
        {
            if (n == null) return;
            InordenRec(n.Izquierda, lista);
            lista.Agregar(n.Dato);
            InordenRec(n.Derecha, lista);
        }
    }
}

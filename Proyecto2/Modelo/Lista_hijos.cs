using System;

namespace Proyecto2.Modelo
{
    // Lista enlazada simple de categorías (hijos), ordenada alfabéticamente
    public class ListaHijos
    {
        public NodoCategoria Raiz { get; set; }
        public int Cantidad { get; set; }

        public ListaHijos()
        {
            Raiz = null;
            Cantidad = 0;
        }

        public bool EstaVacia()
        {
            return Raiz == null;
        }

        // Inserta ordenado alfabéticamente (case-insensitive), sin duplicados
        public bool Insertar(Categoria cat)
        {
            NodoCategoria nuevo = new NodoCategoria(cat);

            if (Raiz == null)
            {
                Raiz = nuevo;
                Cantidad = 1;
                return true;
            }

            int cmp = string.Compare(nuevo.Dato.Nombre, Raiz.Dato.Nombre,
                                     StringComparison.OrdinalIgnoreCase);
            if (cmp == 0) return false;

            if (cmp < 0)
            {
                nuevo.Siguiente = Raiz;
                Raiz = nuevo;
                Cantidad++;
                return true;
            }

            NodoCategoria actual = Raiz;
            while (actual.Siguiente != null)
            {
                int c2 = string.Compare(nuevo.Dato.Nombre,
                                        actual.Siguiente.Dato.Nombre,
                                        StringComparison.OrdinalIgnoreCase);
                if (c2 == 0) return false;
                if (c2 < 0) break;
                actual = actual.Siguiente;
            }

            nuevo.Siguiente = actual.Siguiente;
            actual.Siguiente = nuevo;
            Cantidad++;
            return true;
        }

        public NodoCategoria Buscar(string nombre)
        {
            NodoCategoria actual = Raiz;
            while (actual != null)
            {
                if (string.Equals(actual.Dato.Nombre, nombre,
                                  StringComparison.OrdinalIgnoreCase))
                    return actual;
                actual = actual.Siguiente;
            }
            return null;
        }

        public bool Eliminar(string nombre)
        {
            if (Raiz == null) return false;

            if (string.Equals(Raiz.Dato.Nombre, nombre,
                              StringComparison.OrdinalIgnoreCase))
            {
                Raiz = Raiz.Siguiente;
                Cantidad--;
                return true;
            }

            NodoCategoria actual = Raiz;
            while (actual.Siguiente != null)
            {
                if (string.Equals(actual.Siguiente.Dato.Nombre, nombre,
                                  StringComparison.OrdinalIgnoreCase))
                {
                    actual.Siguiente = actual.Siguiente.Siguiente;
                    Cantidad--;
                    return true;
                }
                actual = actual.Siguiente;
            }
            return false;
        }
    }
}

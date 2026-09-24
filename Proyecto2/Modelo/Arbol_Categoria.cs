using System;

namespace IPC2_Proy02.Modelo
{
    public class ArbolCategorias
    {
        public NodoCategoria Raiz { get; set; }

        public ArbolCategorias()
        {
            Raiz = null;
        }

        public bool EstaVacio()
        {
            return Raiz == null;
        }

        public bool Insertar(string nombreNueva, string nombrePadre)
        {
            if (BuscarNodo(nombreNueva) != null) return false;

            Categoria nueva = new Categoria(nombreNueva);

            if (Raiz == null)
            {
                if (!string.IsNullOrEmpty(nombrePadre)) return false;
                Raiz = new NodoCategoria(nueva);
                return true;
            }

            if (string.IsNullOrEmpty(nombrePadre)) return false;

            NodoCategoria padre = BuscarNodo(nombrePadre);
            if (padre == null) return false;

            return padre.Dato.Hijos.Insertar(nueva);
        }

        public NodoCategoria BuscarNodo(string nombre)
        {
            return BuscarNodoRec(Raiz, nombre);
        }

        private NodoCategoria BuscarNodoRec(NodoCategoria actual, string nombre)
        {
            if (actual == null) return null;

            if (string.Equals(actual.Dato.Nombre, nombre,
                              StringComparison.OrdinalIgnoreCase))
                return actual;

            NodoCategoria hijo = actual.Dato.Hijos.Raiz;
            while (hijo != null)
            {
                NodoCategoria enc = BuscarNodoRec(hijo, nombre);
                if (enc != null) return enc;
                hijo = hijo.Siguiente;
            }
            return null;
        }

        public Categoria Buscar(string nombre)
        {
            NodoCategoria n = BuscarNodo(nombre);
            return n == null ? null : n.Dato;
        }

        // ✅ Ahora devuelve TDA propio ListaCadenas
        public ListaCadenas RecorrerPreorden(string desde)
        {
            ListaCadenas res = new ListaCadenas();
            NodoCategoria inicio = string.IsNullOrEmpty(desde)
                ? Raiz
                : BuscarNodo(desde);
            PreordenRec(inicio, 0, res);
            return res;
        }

        private void PreordenRec(NodoCategoria n, int nivel, ListaCadenas res)
        {
            if (n == null) return;
            res.Agregar(n.Dato.Nombre, nivel);

            NodoCategoria h = n.Dato.Hijos.Raiz;
            while (h != null)
            {
                PreordenRec(h, nivel + 1, res);
                h = h.Siguiente;
            }
        }
    }
}

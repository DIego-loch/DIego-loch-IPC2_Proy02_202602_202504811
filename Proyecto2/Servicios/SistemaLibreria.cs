using System;
using IPC2_Proy02.Modelo;

namespace IPC2_Proy02.Servicios
{
    public class SistemaLibreria
    {
        public ArbolCategorias Categorias { get; private set; }

        public SistemaLibreria()
        {
            Categorias = new ArbolCategorias();
        }

        public void Inicializar()
        {
            Categorias = new ArbolCategorias();
        }

        // ============================================================
        //  CATEGORÍAS
        // ============================================================

        public bool AgregarCategoria(string nombre, string padre)
        {
            return Categorias.Insertar(nombre, padre);
        }

        public Categoria BuscarCategoria(string nombre)
        {
            return Categorias.Buscar(nombre);
        }

        // Devuelve TDA propio ListaCadenas
        public ListaCadenas MostrarEstructura(string desde)
        {
            return Categorias.RecorrerPreorden(desde);
        }

        // ============================================================
        //  LIBROS
        // ============================================================

        public bool AgregarLibro(int isbn, string titulo, string autor, string nombreCategoria)
        {
            Categoria cat = Categorias.Buscar(nombreCategoria);
            if (cat == null) return false;

            Libro libro = new Libro(isbn, titulo, autor, cat);
            return cat.Libros.Insertar(libro);
        }

        public Libro BuscarLibro(int isbn)
        {
            return BuscarLibroRec(Categorias.Raiz, isbn);
        }

        private Libro BuscarLibroRec(NodoCategoria n, int isbn)
        {
            if (n == null) return null;

            Libro enc = n.Dato.Libros.Buscar(isbn);
            if (enc != null) return enc;

            NodoCategoria h = n.Dato.Hijos.Raiz;
            while (h != null)
            {
                Libro r = BuscarLibroRec(h, isbn);
                if (r != null) return r;
                h = h.Siguiente;
            }
            return null;
        }

        public bool EliminarLibro(int isbn)
        {
            return EliminarLibroRec(Categorias.Raiz, isbn);
        }

        private bool EliminarLibroRec(NodoCategoria n, int isbn)
        {
            if (n == null) return false;
            if (n.Dato.Libros.Eliminar(isbn)) return true;

            NodoCategoria h = n.Dato.Hijos.Raiz;
            while (h != null)
            {
                if (EliminarLibroRec(h, isbn)) return true;
                h = h.Siguiente;
            }
            return false;
        }

        // ============================================================
        //  MENOR / MAYOR ISBN (de TODO el catálogo)
        // ============================================================

        public Libro LibroMenor()
        {
            Libro mejor = null;
            MenorRec(Categorias.Raiz, ref mejor);
            return mejor;
        }

        private void MenorRec(NodoCategoria n, ref Libro mejor)
        {
            if (n == null) return;

            Libro m = n.Dato.Libros.Minimo();
            if (m != null && (mejor == null || m.ISBN < mejor.ISBN))
                mejor = m;

            NodoCategoria h = n.Dato.Hijos.Raiz;
            while (h != null)
            {
                MenorRec(h, ref mejor);
                h = h.Siguiente;
            }
        }

        public Libro LibroMayor()
        {
            Libro mejor = null;
            MayorRec(Categorias.Raiz, ref mejor);
            return mejor;
        }

        private void MayorRec(NodoCategoria n, ref Libro mejor)
        {
            if (n == null) return;

            Libro m = n.Dato.Libros.Maximo();
            if (m != null && (mejor == null || m.ISBN > mejor.ISBN))
                mejor = m;

            NodoCategoria h = n.Dato.Hijos.Raiz;
            while (h != null)
            {
                MayorRec(h, ref mejor);
                h = h.Siguiente;
            }
        }

        // ============================================================
        //  TODOS LOS LIBROS ASCENDENTE POR ISBN (TDA propio)
        // ============================================================

        public ListaLibros LibrosAscendente()
        {
            ListaLibros todos = new ListaLibros();
            Recolectar(Categorias.Raiz, todos);
            return MergeSort(todos);
        }

        private void Recolectar(NodoCategoria n, ListaLibros lista)
        {
            if (n == null) return;

            // Agregar los libros de esta categoría (ya vienen en orden)
            ListaLibros librosCat = n.Dato.Libros.Inorden();
            NodoListaLibro a = librosCat.Raiz;
            while (a != null)
            {
                lista.Agregar(a.Dato);
                a = a.Siguiente;
            }

            // Bajar a los hijos
            NodoCategoria h = n.Dato.Hijos.Raiz;
            while (h != null)
            {
                Recolectar(h, lista);
                h = h.Siguiente;
            }
        }

        // ---------- MergeSort recursivo sobre lista enlazada ----------
        private ListaLibros MergeSort(ListaLibros lista)
        {
            if (lista.Raiz == null || lista.Raiz.Siguiente == null)
                return lista;

            // Encontrar el punto medio con lento/rápido
            NodoListaLibro lento = lista.Raiz;
            NodoListaLibro rapido = lista.Raiz.Siguiente;

            while (rapido != null && rapido.Siguiente != null)
            {
                lento = lento.Siguiente;
                rapido = rapido.Siguiente.Siguiente;
            }

            // izquierda = [raiz .. lento]
            // derecha   = [lento.Siguiente .. fin]
            ListaLibros izquierda = new ListaLibros();
            ListaLibros derecha = new ListaLibros();

            NodoListaLibro a = lista.Raiz;
            while (a != null)
            {
                izquierda.Agregar(a.Dato);
                if (a == lento) break;
                a = a.Siguiente;
            }

            a = lento.Siguiente;
            while (a != null)
            {
                derecha.Agregar(a.Dato);
                a = a.Siguiente;
            }

            izquierda = MergeSort(izquierda);
            derecha = MergeSort(derecha);

            return Merge(izquierda, derecha);
        }

        private ListaLibros Merge(ListaLibros a, ListaLibros b)
        {
            ListaLibros res = new ListaLibros();
            NodoListaLibro x = a.Raiz;
            NodoListaLibro y = b.Raiz;

            while (x != null && y != null)
            {
                if (x.Dato.ISBN <= y.Dato.ISBN)
                {
                    res.Agregar(x.Dato);
                    x = x.Siguiente;
                }
                else
                {
                    res.Agregar(y.Dato);
                    y = y.Siguiente;
                }
            }
            while (x != null) { res.Agregar(x.Dato); x = x.Siguiente; }
            while (y != null) { res.Agregar(y.Dato); y = y.Siguiente; }

            return res;
        }

        // ============================================================
        //  LIBROS DE UNA CATEGORÍA ESPECÍFICA
        // ============================================================

        public ListaLibros LibrosDeCategoria(string nombre)
        {
            Categoria cat = Categorias.Buscar(nombre);
            if (cat == null) return new ListaLibros();
            return cat.Libros.Inorden();
        }
    }
}

using System;

namespace Proyecto2.Modelo
{
    public class NodoCadena
    {
        public string Valor { get; set; }
        public int Nivel { get; set; }
        public NodoCadena Siguiente { get; set; }

        public NodoCadena(string valor, int nivel)
        {
            Valor = valor;
            Nivel = nivel;
            Siguiente = null;
        }
    }

    // Lista enlazada simple de cadenas con nivel (para jerarquía de categorías)
    public class ListaCadenas
    {
        public NodoCadena Raiz { get; set; }
        public int Cantidad { get; set; }

        public ListaCadenas()
        {
            Raiz = null;
            Cantidad = 0;
        }

        public bool EstaVacia()
        {
            return Raiz == null;
        }

        public void Agregar(string valor, int nivel)
        {
            NodoCadena nuevo = new NodoCadena(valor, nivel);

            if (Raiz == null)
            {
                Raiz = nuevo;
                Cantidad = 1;
                return;
            }

            NodoCadena actual = Raiz;
            while (actual.Siguiente != null)
                actual = actual.Siguiente;

            actual.Siguiente = nuevo;
            Cantidad++;
        }
    }
}

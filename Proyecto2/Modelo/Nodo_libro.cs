using System;

namespace IPC2_Proy02.Modelo
{
    public class NodoLibro
    {
        public Libro Dato { get; set; }
        public NodoLibro Izquierda { get; set; }
        public NodoLibro Derecha { get; set; }

        public NodoLibro(Libro dato)
        {
            Dato = dato;
            Izquierda = null;
            Derecha = null;
        }
    }
}

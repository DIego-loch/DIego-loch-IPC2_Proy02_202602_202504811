using System;

namespace IPC2_Proy02.Modelo
{
    public class NodoCategoria
    {
        public Categoria Dato { get; set; }
        public NodoCategoria Siguiente { get; set; }

        public NodoCategoria(Categoria dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }
}

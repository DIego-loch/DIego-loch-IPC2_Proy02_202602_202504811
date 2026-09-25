using System;

namespace Proyecto2.Modelo
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

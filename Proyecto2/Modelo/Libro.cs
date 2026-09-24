using System;

namespace IPC2_Proy02.Modelo
{
    public class Libro
    {
        public int ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public Categoria Categoria { get; set; }

        public Libro(int isbn, string titulo, string autor, Categoria categoria)
        {
            ISBN = isbn;
            Titulo = titulo;
            Autor = autor;
            Categoria = categoria;
        }

        public override string ToString()
        {
            return "ISBN: " + ISBN + " | " + Titulo + " | " + Autor;
        }
    }
}

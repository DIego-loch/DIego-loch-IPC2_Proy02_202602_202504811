using System;
using Categoria_libro;
namespace Clase_libro;

public class Libro
{
    public int ISBN { get; set; }
    public string titulo { get; set; }
    public string autor { get; set; }
    public Categoria categoria { get; set; }

    public Libro(int ISBN, string titulo, string autor, Categoria categoria )
    {
        this.ISBN = ISBN;
        this.titulo = titulo;
        this.autor = autor;
        this.categoria = categoria;
    }
}

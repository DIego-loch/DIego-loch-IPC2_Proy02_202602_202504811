using System;
using System.IO;
using System.Text;
using ArbolCategoria;
using Nodo_Categoria;

namespace Servicios;

public class GeneradorGraphviz
{
    public string GenerarDotCategorias(Arbol_Categoria arbol)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("digraph G {");
        sb.AppendLine("    node [shape=box];");

        if (arbol.raiz != null)
            EscribirCategorias(arbol.raiz, sb, ref _contador);

        sb.AppendLine("}");
        return sb.ToString();
    }

    private int _contador = 0;

    private void EscribirCategorias(Nodo_categoria nodo, StringBuilder sb, ref int contador)
    {
        int idActual = contador++;
        string etiqueta = nodo.Nodo_actual.nombre_categoria;

        sb.AppendLine($"    n{idActual} [label=\"{etiqueta}\"];");

        Nodo_categoria? hijo = nodo.Nodo_actual.lista_hijos.raiz;
        while (hijo != null)
        {
            int idHijo = contador;
            EscribirCategorias(hijo, sb, ref contador);
            sb.AppendLine($"    n{idActual} -> n{idHijo};");
            hijo = hijo.siguiente;
        }
    }

    public string GenerarDotLibros(Nodo_categoria categoria)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("digraph G {");
        sb.AppendLine("    node [shape=box];");
        sb.AppendLine($"    label=\"{categoria.Nodo_actual.nombre_categoria}\";");

        if (categoria.Nodo_actual.lista_libros?.raiz != null)
        {
            EscribirLibros(categoria.Nodo_actual.lista_libros.raiz, sb);
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    private void EscribirLibros(Nodo_libro nodo, StringBuilder sb)
    {
        sb.AppendLine($"    l{nodo.Nodo_actual.ISBN} [label=\"{nodo.Nodo_actual.ISBN}\\n{nodo.Nodo_actual.titulo}\"];");

        if (nodo.Nodo_izquierda != null)
        {
            sb.AppendLine($"    l{nodo.Nodo_actual.ISBN} -> l{nodo.Nodo_izquierda.Nodo_actual.ISBN};");
            EscribirLibros(nodo.Nodo_izquierda, sb);
        }
        if (nodo.Nodo_derecha != null)
        {
            sb.AppendLine($"    l{nodo.Nodo_actual.ISBN} -> l{nodo.Nodo_derecha.Nodo_actual.ISBN};");
            EscribirLibros(nodo.Nodo_derecha, sb);
        }
    }

    public void Guardar(string contenido, string ruta)
    {
        File.WriteAllText(ruta, contenido);
    }
}

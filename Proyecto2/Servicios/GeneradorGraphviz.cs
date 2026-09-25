using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Proyecto2.Modelo;

namespace Proyecto2.Servicios
{
    public class GeneradorGraphviz
    {
        private string carpetaSalida;

        public GeneradorGraphviz(string carpeta)
        {
            carpetaSalida = carpeta;
            if (!Directory.Exists(carpetaSalida))
                Directory.CreateDirectory(carpetaSalida);
        }

        // ============================================================
        //  ÁRBOL DE CATEGORÍAS → .dot / .png
        // ============================================================

        public string GenerarArbolCategorias(ArbolCategorias arbol)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph G {");
            sb.AppendLine("  node [shape=box, style=filled, fillcolor=\"#B3E5FC\"];");
            sb.AppendLine("  rankdir=TB;");

            int contador = 0;
            if (arbol.Raiz != null)
                Recorrer(arbol.Raiz, ref contador, sb, -1);

            sb.AppendLine("}");
            return GuardarYRenderizar(sb.ToString(), "categorias");
        }

        private int Recorrer(NodoCategoria n, ref int contador,
                             StringBuilder sb, int padreId)
        {
            if (n == null) return -1;

            int id = contador++;
            sb.AppendLine("  n" + id + " [label=\"" +
                          Escapar(n.Dato.Nombre) + "\"];");

            if (padreId >= 0)
                sb.AppendLine("  n" + padreId + " -> n" + id + ";");

            NodoCategoria h = n.Dato.Hijos.Raiz;
            while (h != null)
            {
                Recorrer(h, ref contador, sb, id);
                h = h.Siguiente;
            }
            return id;
        }

        // ============================================================
        //  BST DE LIBROS → .dot / .png
        // ============================================================

        public string GenerarArbolLibros(ArbolLibros arbol, string nombreCat)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph G {");
            sb.AppendLine("  node [shape=record, style=filled, fillcolor=\"#C8E6C9\"];");
            sb.AppendLine("  label=\"Libros en: " + Escapar(nombreCat) + "\";");
            sb.AppendLine("  labelloc=t;");

            int contador = 0;
            if (arbol.Raiz != null)
                RecorrerLibro(arbol.Raiz, ref contador, sb);

            sb.AppendLine("}");
            return GuardarYRenderizar(sb.ToString(),
                                      "libros_" + Sanitizar(nombreCat));
        }

        private void RecorrerLibro(NodoLibro n, ref int contador, StringBuilder sb)
        {
            if (n == null) return;

            int id = contador++;
            string label = "{ ISBN: " + n.Dato.ISBN +
                           " | " + Escapar(n.Dato.Titulo) +
                           " | " + Escapar(n.Dato.Autor) + " }";

            sb.AppendLine("  n" + id + " [label=\"" + label + "\"];");

            if (n.Izquierda != null)
            {
                int idIzq = contador;
                RecorrerLibro(n.Izquierda, ref contador, sb);
                sb.AppendLine("  n" + id + " -> n" + idIzq + ";");
            }
            if (n.Derecha != null)
            {
                int idDer = contador;
                RecorrerLibro(n.Derecha, ref contador, sb);
                sb.AppendLine("  n" + id + " -> n" + idDer + ";");
            }
        }

        // ============================================================
        //  UTILIDADES
        // ============================================================

        private string GuardarYRenderizar(string dot, string nombre)
        {
            string rutaDot = Path.Combine(carpetaSalida, nombre + ".dot");
            string rutaPng = Path.Combine(carpetaSalida, nombre + ".png");

            File.WriteAllText(rutaDot, dot);

            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "dot";
                p.StartInfo.Arguments = "-Tpng \"" + rutaDot +
                                        "\" -o \"" + rutaPng + "\"";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
            }
            catch
            {
                // Graphviz no instalado o no en PATH: se genera solo el .dot
            }

            return "/Reportes/" + nombre + ".png";
        }

        private string Escapar(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\n", " ");
        }

        private string Sanitizar(string s)
        {
            if (string.IsNullOrEmpty(s)) return "sin_nombre";

            char[] invalidos = Path.GetInvalidFileNameChars();
            string res = s;
            foreach (char c in invalidos)
                res = res.Replace(c, '_');
            return res;
        }
    }
}

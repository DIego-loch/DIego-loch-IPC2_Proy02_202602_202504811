using System;
using System.IO;
using System.Xml;

namespace IPC2_Proy02.Servicios
{
    public class CargadorXML
    {
        private SistemaLibreria sistema;

        public CargadorXML(SistemaLibreria s)
        {
            sistema = s;
        }

        // Devuelve un string con el resultado (éxito o error)
        public string Cargar(string ruta)
        {
            if (!File.Exists(ruta))
                return "Archivo no encontrado: " + ruta;

            int nCats = 0;
            int nLibs = 0;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ruta);

                // ---------- listaCategorias ----------
                XmlNodeList cats = doc.SelectNodes("//listaCategorias/categoria");
                if (cats != null)
                {
                    foreach (XmlNode c in cats)
                    {
                        string nombre = "";
                        string padre = null;

                        if (c.Attributes["nombre"] != null)
                            nombre = c.Attributes["nombre"].Value;

                        if (c.Attributes["padre"] != null)
                            padre = c.Attributes["padre"].Value;

                        if (string.IsNullOrWhiteSpace(nombre)) continue;

                        if (sistema.AgregarCategoria(nombre, padre))
                            nCats++;
                    }
                }

                // ---------- listaLibros ----------
                XmlNodeList libs = doc.SelectNodes("//listaLibros/libro");
                if (libs != null)
                {
                    foreach (XmlNode l in libs)
                    {
                        if (l.Attributes["isbn"] == null) continue;

                        int isbn = int.Parse(l.Attributes["isbn"].Value);

                        XmlNode nTitulo = l.SelectSingleNode("titulo");
                        XmlNode nAutor = l.SelectSingleNode("autor");
                        XmlNode nCategoria = l.SelectSingleNode("categoria");

                        string titulo = nTitulo != null ? nTitulo.InnerText : "";
                        string autor = nAutor != null ? nAutor.InnerText : "";
                        string categoria = nCategoria != null ? nCategoria.InnerText : "";

                        if (sistema.AgregarLibro(isbn, titulo, autor, categoria))
                            nLibs++;
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al cargar XML: " + ex.Message;
            }

            return "OK - Categorias: " + nCats + " | Libros: " + nLibs;
        }
    }
}

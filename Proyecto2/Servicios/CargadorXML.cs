using System;
using System.IO;
using System.Xml;

namespace Proyecto2.Servicios
{
    public class CargadorXML
    {
        private SistemaLibreria sistema;

        public CargadorXML(SistemaLibreria s)
        {
            sistema = s;
        }

        // Cargar por ruta (endpoint /cargar-xml)
        public string Cargar(string ruta)
        {
            if (!File.Exists(ruta))
                return "Archivo no encontrado: " + ruta;

            using (FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read))
            {
                return CargarDesdeStream(fs);
            }
        }

        // Cargar desde un stream (endpoint /subir-xml)
        public string CargarDesdeStream(Stream stream)
        {
            int nCats = 0;
            int nLibs = 0;
            int rechazadas = 0;
            int rechazados = 0;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(stream);

                // ---------------- CATEGORÍAS ----------------
                XmlNodeList cats = doc.SelectNodes("//listaCategorias/categoria");
                if (cats == null || cats.Count == 0)
                    cats = doc.SelectNodes("//listacategorias/categoria");

                if (cats != null)
                {
                    foreach (XmlNode c in cats)
                    {
                        string nombre = null;
                        string padre = null;

                        // 1) Atributo nombre (entrada.xml)
                        if (c.Attributes["nombre"] != null)
                            nombre = c.Attributes["nombre"].Value;

                        // 2) Texto interno (entrada_100.xml)
                        if (string.IsNullOrWhiteSpace(nombre))
                            nombre = c.InnerText != null ? c.InnerText.Trim() : null;

                        // Padre
                        if (c.Attributes["padre"] != null)
                            padre = c.Attributes["padre"].Value;

                        if (string.IsNullOrWhiteSpace(nombre))
                            continue;

                        if (sistema.AgregarCategoria(nombre, padre))
                            nCats++;
                        else
                            rechazadas++;
                    }
                }

                // ---------------- LIBROS ----------------
                XmlNodeList libs = doc.SelectNodes("//listaLibros/libro");
                if (libs == null || libs.Count == 0)
                    libs = doc.SelectNodes("//listalibros/libro");

                if (libs != null)
                {
                    foreach (XmlNode l in libs)
                    {
                        // ISBN: atributo o nodo hijo
                        int isbn = 0;

                        if (l.Attributes["isbn"] != null)
                            int.TryParse(l.Attributes["isbn"].Value, out isbn);
                        else if (l.Attributes["ISBN"] != null)
                            int.TryParse(l.Attributes["ISBN"].Value, out isbn);
                        else
                        {
                            XmlNode nIsbn = l.SelectSingleNode("ISBN");
                            if (nIsbn == null) nIsbn = l.SelectSingleNode("isbn");
                            if (nIsbn != null)
                                int.TryParse(nIsbn.InnerText.Trim(), out isbn);
                        }

                        if (isbn <= 0) { rechazados++; continue; }

                        XmlNode nTitulo = l.SelectSingleNode("titulo");
                        if (nTitulo == null) nTitulo = l.SelectSingleNode("Titulo");
                        string titulo = nTitulo != null ? nTitulo.InnerText.Trim() : "";

                        XmlNode nAutor = l.SelectSingleNode("autor");
                        if (nAutor == null) nAutor = l.SelectSingleNode("Autor");
                        string autor = nAutor != null ? nAutor.InnerText.Trim() : "";

                        XmlNode nCat = l.SelectSingleNode("categoria");
                        if (nCat == null) nCat = l.SelectSingleNode("Categoria");
                        string categoria = nCat != null ? nCat.InnerText.Trim() : "";

                        if (sistema.AgregarLibro(isbn, titulo, autor, categoria))
                            nLibs++;
                        else
                            rechazados++;
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al cargar XML: " + ex.Message;
            }

            return "OK - Categorías: " + nCats +
                   " | Libros: " + nLibs +
                   " | Rechazadas: " + rechazadas +
                   " | Rechazados: " + rechazados;
        }
    }
}

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

        public string Cargar(string ruta)
        {
            if (!File.Exists(ruta))
                return "Archivo no encontrado: " + ruta;

            using (FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read))
            {
                return CargarDesdeStream(fs);
            }
        }

        public string CargarDesdeStream(Stream stream)
        {
            int nCats = 0, nLibs = 0;
            int catsRechazadas = 0, libsRechazados = 0;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(stream);

                // ============ CATEGORÍAS (2 PASADAS) ============
                XmlNodeList cats = doc.SelectNodes("//listaCategorias/categoria");
                if (cats == null || cats.Count == 0)
                    cats = doc.SelectNodes("//listacategorias/categoria");

                if (cats != null)
                {
                    // ---- PASO 1: insertar las que se puedan ----
                    for (int i = 0; i < cats.Count; i++)
                    {
                        XmlNode c = cats[i];
                        string nombre = ExtraerNombreCat(c);
                        string padre = c.Attributes["padre"] != null
                            ? c.Attributes["padre"].Value : null;

                        if (string.IsNullOrWhiteSpace(nombre)) continue;

                        if (sistema.AgregarCategoria(nombre, padre))
                            nCats++;
                    }

                    // ---- PASO 2: reintentar huérfanas ----
                    bool huboCambios = true;
                    int iteracion = 0;
                    while (huboCambios && iteracion < 10)
                    {
                        huboCambios = false;
                        iteracion++;

                        for (int i = 0; i < cats.Count; i++)
                        {
                            XmlNode c = cats[i];
                            string nombre = ExtraerNombreCat(c);
                            string padre = c.Attributes["padre"] != null
                                ? c.Attributes["padre"].Value : null;

                            if (string.IsNullOrWhiteSpace(nombre)) continue;
                            if (sistema.BuscarCategoria(nombre) != null) continue;

                            if (string.IsNullOrEmpty(padre))
                            {
                                if (sistema.AgregarCategoria(nombre, null))
                                {
                                    nCats++;
                                    huboCambios = true;
                                }
                                continue;
                            }

                            if (sistema.BuscarCategoria(padre) != null)
                            {
                                if (sistema.AgregarCategoria(nombre, padre))
                                {
                                    nCats++;
                                    huboCambios = true;
                                }
                            }
                        }
                    }

                    catsRechazadas = cats.Count - nCats;
                }

                // ============ LIBROS ============
                XmlNodeList libs = doc.SelectNodes("//listaLibros/libro");
                if (libs == null || libs.Count == 0)
                    libs = doc.SelectNodes("//listalibros/libro");

                if (libs != null)
                {
                    foreach (XmlNode l in libs)
                    {
                        int isbn = ExtraerIsbn(l);
                        if (isbn <= 0) { libsRechazados++; continue; }

                        XmlNode nT = l.SelectSingleNode("titulo");
                        XmlNode nA = l.SelectSingleNode("autor");
                        XmlNode nC = l.SelectSingleNode("categoria");

                        string titulo = nT != null ? nT.InnerText.Trim() : "";
                        string autor = nA != null ? nA.InnerText.Trim() : "";
                        string categoria = nC != null ? nC.InnerText.Trim() : "";

                        if (sistema.AgregarLibro(isbn, titulo, autor, categoria))
                            nLibs++;
                        else
                            libsRechazados++;
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al cargar XML: " + ex.Message;
            }

            return "OK - Categorías: " + nCats +
                   " | Libros: " + nLibs +
                   " | Categorías rechazadas: " + catsRechazadas +
                   " | Libros rechazados: " + libsRechazados;
        }

        private string ExtraerNombreCat(XmlNode c)
        {
            string nombre = null;

            if (c.Attributes["nombre"] != null)
                nombre = c.Attributes["nombre"].Value;

            if (string.IsNullOrWhiteSpace(nombre) && c.InnerText != null)
                nombre = c.InnerText.Trim();

            return nombre;
        }

        private int ExtraerIsbn(XmlNode l)
        {
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

            return isbn;
        }
    }
}

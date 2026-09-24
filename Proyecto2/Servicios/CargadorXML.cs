   using System;
   using System.Xml;
   using System.IO;
   using ArbolCategoria;
   using Categoria_libro;
   using Clase_libro;
   using Nodo_Categoria;

   namespace Servicios;

   public class CargadorXML
   {
       public bool CargarDesdeRuta(string ruta, Arbol_Categoria arbol)
       {
           if (!File.Exists(ruta))
           {
               Console.WriteLine("Archivo no encontrado: " + ruta);
               return false;
           }

           XmlDocument doc = new XmlDocument();
           doc.Load(ruta);
           return ProcesarDocumento(doc, arbol);
       }

       public bool CargarDesdeStream(Stream stream, Arbol_Categoria arbol)
       {
           try
           {
               XmlDocument doc = new XmlDocument();
               doc.Load(stream);
               return ProcesarDocumento(doc, arbol);
           }
           catch (Exception e)
           {
               Console.WriteLine("Error leyendo XML: " + e.Message);
               return false;
           }
       }

       private bool ProcesarDocumento(XmlDocument doc, Arbol_Categoria arbol)
       {
           XmlNode? raizXml = doc.DocumentElement;
           if (raizXml == null)
           {
               Console.WriteLine("XML vacío.");
               return false;
           }

           XmlNode? listaCategorias = raizXml.SelectSingleNode("listaCategorias");
           if (listaCategorias != null)
           {
               foreach (XmlNode catXml in listaCategorias.SelectNodes("categoria")!)
               {
                   ProcesarCategoria(catXml, null, arbol);
               }
           }

           XmlNode? listaLibros = raizXml.SelectSingleNode("listaLibros");
           if (listaLibros != null)
           {
               foreach (XmlNode libroXml in listaLibros.SelectNodes("libro")!)
               {
                   ProcesarLibro(libroXml, arbol);
               }
           }

           return true;
       }

       private void ProcesarCategoria(XmlNode catXml, Nodo_categoria? padre, Arbol_Categoria arbol)
       {
           string? nombre = catXml.Attributes?["nombre"]?.Value;
           if (string.IsNullOrEmpty(nombre)) return;

           Nodo_categoria? nodoActual = null;

           if (arbol.EstaVacio())
           {
               arbol.InsertarRaiz(nombre);
               nodoActual = arbol.Buscar(nombre);
           }
           else if (arbol.Existe(nombre))
           {
               nodoActual = arbol.Buscar(nombre);
           }
           else
           {
               string nombrePadre = padre != null
                   ? padre.Nodo_actual.nombre_categoria
                   : arbol.raiz!.Nodo_actual.nombre_categoria;
               arbol.Insertar(nombre, nombrePadre);
               nodoActual = arbol.Buscar(nombre);
           }

           if (nodoActual == null) return;

           XmlNodeList? hijos = catXml.SelectNodes("categoria");
           if (hijos != null)
           {
               foreach (XmlNode hijoXml in hijos)
                   ProcesarCategoria(hijoXml, nodoActual, arbol);
           }
       }

       private void ProcesarLibro(XmlNode libroXml, Arbol_Categoria arbol)
       {
           string? isbnStr = libroXml.Attributes?["isbn"]?.Value;
           string? titulo = libroXml.Attributes?["titulo"]?.Value;
           string? autor = libroXml.Attributes?["autor"]?.Value;
           string? nombreCategoria = libroXml.SelectSingleNode("categoria")?.InnerText?.Trim();

           if (isbnStr == null || titulo == null || autor == null || nombreCategoria == null) return;
           if (!int.TryParse(isbnStr, out int isbn)) return;

           Nodo_categoria? nodoCategoria = arbol.Buscar(nombreCategoria);
           if (nodoCategoria == null)
           {
               Console.WriteLine("Categoría no encontrada: " + nombreCategoria);
               return;
           }

           if (nodoCategoria.Nodo_actual.lista_libros!.BuscarInfo(isbn) != null) return;

           Libro libro = new Libro(isbn, titulo, autor, nodoCategoria.Nodo_actual);
           nodoCategoria.Nodo_actual.lista_libros.IngresarLibro(libro);
       }
   }

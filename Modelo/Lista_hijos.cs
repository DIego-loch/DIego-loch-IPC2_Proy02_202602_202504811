using System;
using Nodo_Categoria;
using Categoria_libro;

namespace List_enlazada;

public class Lista_hijos
{
    public Nodo_categoria? raiz { get; set; }

    public Lista_hijos()
    {
        raiz = null;
    }

    // Inserta una categoría ordenada alfabéticamente
    public void Ingreso_lista(Categoria categoria)
    {
        Nodo_categoria nuevo = new Nodo_categoria(categoria);

        // Caso 1: lista vacía
        if (raiz == null)
        {
            raiz = nuevo;
            return;
        }

        // Caso 2: el nuevo va antes que la cabeza
        if (string.Compare(nuevo.Nodo_actual.nombre_categoria, raiz.Nodo_actual.nombre_categoria) < 0)
        {
            nuevo.siguiente = raiz;
            raiz = nuevo;
            return;
        }

        // Caso 3: el nuevo va en medio o al final
        Nodo_categoria actual = raiz;
        while (actual.siguiente != null &&
               string.Compare(actual.siguiente.Nodo_actual.nombre_categoria, nuevo.Nodo_actual.nombre_categoria) < 0)
        {
            actual = actual.siguiente;
        }

        // Insertar entre actual y actual.siguiente
        nuevo.siguiente = actual.siguiente;
        actual.siguiente = nuevo;
    }

    // Busca un hijo por nombre
    public Nodo_categoria? Buscar(string nombre)
    {
        Nodo_categoria actual = raiz;
        while (actual != null)
        {
            if (actual.Nodo_actual.nombre_categoria == nombre)
                return actual;
            actual = actual.siguiente;
        }
        return null;
    }

    // Verifica si la lista está vacía
    public bool EstaVacia()
    {
        return raiz == null;
    }

    // Muestra todos los hijos (para debug)
    public void Mostrar_lista()
    {
        Nodo_categoria aux = raiz;
        while (aux != null)
        {
            Console.WriteLine(aux.Nodo_actual.nombre_categoria);
            aux = aux.siguiente;
        }
    }
}

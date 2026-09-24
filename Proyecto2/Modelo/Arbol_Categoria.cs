using System;
using Categoria_libro;
using Nodo_Categoria;

namespace ArbolCategoria;

public class Arbol_Categoria
{
    public Nodo_categoria? raiz { get; set; }

    public Arbol_Categoria()
    {
        raiz = null;
    }

    // Verifica si el árbol está vacío
    public bool EstaVacio()
    {
        return raiz == null;
    }

    // Inserta una categoría como hijo de un padre (o como raíz si está vacío)
    public bool Insertar(string nombreNueva, string nombrePadre)
    {
        // Caso 1: árbol vacío → la nueva es la raíz
        if (raiz == null)
        {
            raiz = new Nodo_categoria(new Categoria(nombreNueva));
            return true;
        }

        // Verificar que no exista ya (duplicado global)
        if (Existe(nombreNueva))
        {
            Console.WriteLine("Ya existe una categoría con ese nombre: " + nombreNueva);
            return false;
        }

        // Buscar el padre
        Nodo_categoria? padre = Buscar(nombrePadre);
        if (padre == null)
        {
            Console.WriteLine("Padre no encontrado: " + nombrePadre);
            return false;
        }

        // Insertar en la lista de hijos del padre
        padre.Nodo_actual.lista_hijos.Ingreso_lista(new Categoria(nombreNueva));
        return true;
    }

    // Inserta una categoría como raíz (si el árbol está vacío)
    public bool InsertarRaiz(string nombre)
    {
        if (raiz != null) return false;

        raiz = new Nodo_categoria(new Categoria(nombre));
        return true;
    }

    // Busca una categoría por nombre (recursivo)
    public Nodo_categoria? Buscar(string nombre)
    {
        return BuscarRecursivo(raiz, nombre);
    }

    private Nodo_categoria? BuscarRecursivo(Nodo_categoria? actual, string nombre)
    {
        if (actual == null) return null;

        if (actual.Nodo_actual.nombre_categoria == nombre)
            return actual;

        // Recorrer los hijos
        Nodo_categoria? hijo = actual.Nodo_actual.lista_hijos.raiz;
        while (hijo != null)
        {
            Nodo_categoria? encontrado = BuscarRecursivo(hijo, nombre);
            if (encontrado != null) return encontrado;
            hijo = hijo.siguiente;
        }

        return null;
    }

    // Verifica si una categoría ya existe
    public bool Existe(string nombre)
    {
        return Buscar(nombre) != null;
    }

    // Recorrido preorden: muestra toda la jerarquía
    public void Recorrer()
    {
        RecorrerRecursivo(raiz, 0);
    }

    private void RecorrerRecursivo(Nodo_categoria? actual, int nivel)
    {
        if (actual == null) return;

        // Sangría según nivel
        for (int i = 0; i < nivel; i++) Console.Write("   ");
        Console.WriteLine(actual.Nodo_actual.nombre_categoria);

        // Recorrer hijos
        Nodo_categoria? hijo = actual.Nodo_actual.lista_hijos.raiz;
        while (hijo != null)
        {
            RecorrerRecursivo(hijo, nivel + 1);
            hijo = hijo.siguiente;
        }
    }

    // Recorrer a partir de una categoría específica
    public void RecorrerDesde(string nombre)
    {
        Nodo_categoria? nodo = Buscar(nombre);
        if (nodo == null)
        {
            Console.WriteLine("Categoría no encontrada: " + nombre);
            return;
        }
        RecorrerRecursivo(nodo, 0);
    }
}

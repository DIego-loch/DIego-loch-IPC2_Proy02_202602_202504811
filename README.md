

``````mermaid
classDiagram
    %% ==========================================
    %% CAPA DE MODELO
    %% ==========================================

    class Libro {
        +int ISBN
        +string Titulo
        +string Autor
        +Categoria Categoria
        +Libro(int isbn, string titulo, string autor, Categoria categoria)
        +ToString() string
    }

    class Categoria {
        +string Nombre
        +ListaHijos Hijos
        +ArbolLibros Libros
        +Categoria(string nombre)
        +ToString() string
    }

    class NodoCategoria {
        +Categoria Dato
        +NodoCategoria Siguiente
        +NodoCategoria(Categoria dato)
    }

    class NodoLibro {
        +Libro Dato
        +NodoLibro Izquierda
        +NodoLibro Derecha
        +NodoLibro(Libro dato)
    }

    class NodoListaLibro {
        +Libro Dato
        +NodoListaLibro Siguiente
        +NodoListaLibro(Libro dato)
    }

    class NodoCadena {
        +string Valor
        +int Nivel
        +NodoCadena Siguiente
        +NodoCadena(string valor, int nivel)
    }

    class ListaHijos {
        +NodoCategoria Raiz
        +int Cantidad
        +ListaHijos()
        +bool EstaVacia()
        +bool Insertar(Categoria cat)
        +NodoCategoria Buscar(string nombre)
        +bool Eliminar(string nombre)
    }

    class ArbolLibros {
        +NodoLibro Raiz
        +ArbolLibros()
        +bool EstaVacio()
        +bool Insertar(Libro libro)
        +Libro Buscar(int isbn)
        +Libro Minimo()
        +Libro Maximo()
        +bool Eliminar(int isbn)
        +ListaLibros Inorden()
    }

    class ArbolCategorias {
        +NodoCategoria Raiz
        +ArbolCategorias()
        +bool EstaVacio()
        +bool Insertar(string nombreNueva, string nombrePadre)
        +NodoCategoria BuscarNodo(string nombre)
        +Categoria Buscar(string nombre)
        +ListaCadenas RecorrerPreorden(string desde)
    }

    class ListaLibros {
        +NodoListaLibro Raiz
        +int Cantidad
        +ListaLibros()
        +bool EstaVacia()
        +void Agregar(Libro libro)
    }

    class ListaCadenas {
        +NodoCadena Raiz
        +int Cantidad
        +ListaCadenas()
        +bool EstaVacia()
        +void Agregar(string valor, int nivel)
    }

    %% ==========================================
    %% CAPA DE SERVICIOS
    %% ==========================================

    class SistemaLibreria {
        +ArbolCategorias Categorias
        +SistemaLibreria()
        +void Inicializar()
        +bool AgregarCategoria(string nombre, string padre)
        +Categoria BuscarCategoria(string nombre)
        +ListaCadenas MostrarEstructura(string desde)
        +bool AgregarLibro(int isbn, string titulo, string autor, string categoria)
        +Libro BuscarLibro(int isbn)
        +bool EliminarLibro(int isbn)
        +Libro LibroMenor()
        +Libro LibroMayor()
        +ListaLibros LibrosAscendente()
        +ListaLibros LibrosDeCategoria(string nombre)
    }

    class CargadorXML {
        -SistemaLibreria sistema
        +CargadorXML(SistemaLibreria s)
        +string Cargar(string ruta)
        +string CargarDesdeStream(Stream stream)
    }

    class GeneradorGraphviz {
        -string carpetaSalida
        +GeneradorGraphviz(string carpeta)
        +string GenerarArbolCategorias(ArbolCategorias arbol)
        +string GenerarArbolLibros(ArbolLibros arbol, string nombreCat)
    }

    %% ==========================================
    %% CAPA DE CONTROLADORES
    %% ==========================================

    class LibrosController {
        -SistemaLibreria _sistema
        -GeneradorGraphviz _gv
        +LibrosController(SistemaLibreria, GeneradorGraphviz)
        +IActionResult Get()
        +IActionResult Get(int isbn)
        +IActionResult Post(LibroDTO dto)
        +IActionResult Delete(int isbn)
        +IActionResult Menor()
        +IActionResult Mayor()
        +IActionResult PorCategoria(string nombre)
    }

    class CategoriasController {
        -SistemaLibreria _sistema
        -GeneradorGraphviz _gv
        +CategoriasController(SistemaLibreria, GeneradorGraphviz)
        +IActionResult Get(string desde)
        +IActionResult Post(CategoriaDTO dto)
        +IActionResult Grafo()
    }

    class SistemaController {
        -SistemaLibreria _sistema
        -CargadorXML _cargador
        +SistemaController(SistemaLibreria, CargadorXML)
        +IActionResult Inicializar()
        +IActionResult CargarXml(RutaDTO dto)
        +IActionResult SubirXml(IFormFile archivo)
    }

    class LibroDTO {
        +int ISBN
        +string Titulo
        +string Autor
        +string Categoria
    }

    class CategoriaDTO {
        +string Nombre
        +string Padre
    }

    class RutaDTO {
        +string Ruta
    }

    %% ==========================================
    %% RELACIONES
    %% ==========================================

    %% Composición: Categoria tiene una lista de hijos y un árbol de libros
    Categoria *-- ListaHijos : hijos
    Categoria *-- ArbolLibros : libros

    %% Cada libro pertenece a una categoría
    Libro --> Categoria : pertenece a

    %% Nodos usados por las listas
    ListaHijos o-- NodoCategoria : contiene
    ListaLibros o-- NodoListaLibro : contiene
    ListaCadenas o-- NodoCadena : contiene

    %% Árboles usan nodos
    ArbolLibros o-- NodoLibro : contiene
    ArbolCategorias o-- NodoCategoria : contiene

    %% Nodos referencian a sus datos
    NodoCategoria --> Categoria
    NodoLibro --> Libro
    NodoListaLibro --> Libro
    NodoCadena ..> NodoCadena : Siguiente

    %% Relaciones recursivas (auto-referencia)
    NodoCategoria --> NodoCategoria : Siguiente
    NodoLibro --> NodoLibro : Izquierda/Derecha
    NodoListaLibro --> NodoListaLibro : Siguiente

    %% Servicios usan el modelo
    SistemaLibreria *-- ArbolCategorias : contiene
    SistemaLibreria ..> ListaLibros : devuelve
    SistemaLibreria ..> ListaCadenas : devuelve

    %% Servicios auxiliares
    CargadorXML --> SistemaLibreria : usa
    GeneradorGraphviz ..> ArbolCategorias : recibe
    GeneradorGraphviz ..> ArbolLibros : recibe

    %% Controladores usan servicios
    LibrosController --> SistemaLibreria : usa
    LibrosController --> GeneradorGraphviz : usa
    LibrosController ..> LibroDTO : recibe

    CategoriasController --> SistemaLibreria : usa
    CategoriasController --> GeneradorGraphviz : usa
    CategoriasController ..> CategoriaDTO : recibe

    SistemaController --> SistemaLibreria : usa
    SistemaController --> CargadorXML : usa
    SistemaController ..> RutaDTO : recibe

    %% Notas
    note for ArbolCategorias "Árbol N-ario para jerarquía\nde categorías y subcategorías"
    note for ArbolLibros "BST indexado por ISBN\ndentro de cada categoría"
    note for ListaHijos "Lista ordenada alfabéticamente\nde hijos"
    note for ListaLibros "Lista auxiliar usada\npara reportes y resultados"
``` 

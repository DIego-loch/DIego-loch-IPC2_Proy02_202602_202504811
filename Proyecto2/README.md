# SISTEMA DE GESTIÓN DE CATÁLOGO BIBLIOGRÁFICO JERÁRQUICO USANDO ÁRBOLES N-ARIOS Y ÁRBOLES BINARIOS DE BÚSQUEDA

Proyecto 2

Introducción a la Programación y Computación 2

Autor: Diego Alexander Loch Cocón
Carnet: 202504811

Guatemala, septiembre de 2026

---

## Resumen

El presente proyecto aborda el desarrollo de un sistema de gestión de catálogo bibliográfico para una cadena de librerías, el cual permite administrar más de 100,000 libros distribuidos jerárquicamente en categorías y subcategorías. El sistema se implementa utilizando el lenguaje de programación C# bajo el paradigma de programación orientada a objetos, empleando exclusivamente Tipos de Datos Abstractos (TDA) desarrollados por el estudiante, tales como árboles n-arios, árboles binarios de búsqueda y listas enlazadas simples, con el fin de almacenar y manipular la información de manera eficiente incluso cuando el catálogo crece significativamente. La solución incluye la carga incremental de archivos de configuración en formato XML para registrar categorías y libros, así como la generación de reportes gráficos utilizando Graphviz para visualizar tanto la estructura jerárquica del catálogo como la organización interna de los libros en cada categoría. El sistema logra mantener tiempos de búsqueda reducidos gracias al uso de un árbol binario de búsqueda indexado por ISBN, y permite identificar rápidamente qué libros pertenecen a una categoría específica, cuál es el libro con el menor y mayor ISBN, y cuál es el orden ascendente de todos los libros del catálogo. Los resultados obtenidos demuestran que la solución es capaz de procesar archivos de entrada con miles de registros en tiempos aceptables y de representar gráficamente la organización completa del catálogo.

Palabras clave: Tipos de Datos Abstractos, programación orientada a objetos, Graphviz, XML, árbol n-ario, árbol binario de búsqueda, catálogo bibliográfico.

## Abstract

This project addresses the development of a bibliographic catalog management system for a bookstore chain, which allows the administration of more than 100,000 books hierarchically distributed in categories and subcategories. The system is implemented using the C# programming language under the object-oriented programming paradigm, exclusively using Abstract Data Types (ADT) developed by the student, such as n-ary trees, binary search trees and singly linked lists, in order to efficiently store and manipulate information even when the catalog grows significantly. The solution includes the incremental loading of XML configuration files to register categories and books, as well as the generation of graphic reports using Graphviz to visualize both the hierarchical structure of the catalog and the internal organization of the books in each category. The system manages to maintain reduced search times thanks to the use of a binary search tree indexed by ISBN, and allows to quickly identify which books belong to a specific category, which is the book with the lowest and highest ISBN, and which is the ascending order of all books in the catalog. The obtained results demonstrate that the solution is capable of processing input files with thousands of records in acceptable times and of graphically representing the complete organization of the catalog.

Keywords: Abstract Data Types, object-oriented programming, Graphviz, XML, n-ary tree, binary search tree, bibliographic catalog.

---

## 1. Introducción

En la actualidad, las cadenas de librerías manejan volúmenes de información que superan con facilidad los cientos de miles de registros, distribuidos en múltiples sucursales y organizados en secciones, categorías y subcategorías temáticas. La gestión manual de este tipo de catálogos resulta inviable, ya que los empleados invierten demasiado tiempo localizando libros, determinando su ubicación exacta dentro de la jerarquía de categorías y estableciendo relaciones temáticas entre obras. La tecnología ofrece alternativas eficientes para resolver este problema, entre ellas el uso de estructuras de datos dinámicas que permiten representar relaciones jerárquicas y realizar búsquedas en tiempos logarítmicos.

El presente proyecto tiene como objetivo modelar, documentar e implementar una solución que permita a una cadena de librerías administrar de manera eficiente su catálogo bibliográfico. Para ello, se desarrolla un sistema de control en C# que procesa archivos XML de configuración de forma incremental, utiliza estructuras de datos dinámicas implementadas desde cero y genera reportes gráficos con Graphviz. El sistema debe ser capaz de registrar nuevos libros, buscar títulos por ISBN, mostrar la organización jerárquica completa del catálogo, identificar los libros asociados a una categoría específica, obtener el libro con el menor y el mayor ISBN, listar todos los libros en orden ascendente por ISBN y eliminar libros que ya no se encuentren disponibles.

## 2. Desarrollo del tema

### 2.1 Tipos de Datos Abstractos (TDA)

El proyecto requiere el uso exclusivo de Tipos de Datos Abstractos (TDA) implementados por el estudiante, sin utilizar las colecciones nativas del lenguaje C#. Esta restricción tiene como objetivo fortalecer la comprensión de las estructuras de datos y su funcionamiento interno, así como garantizar que el estudiante sea capaz de diseñar y construir soluciones propias a partir de los conceptos fundamentales de la programación.

#### 2.1.1 Nodo

El TDA Nodo es la base fundamental para la construcción de todas las demás estructuras. En el proyecto se emplean dos variantes de nodo: el nodo de categoría, que almacena una referencia a un objeto de tipo Categoria y un puntero al siguiente hermano dentro de una lista enlazada; y el nodo de libro, que almacena una referencia a un objeto de tipo Libro, además de referencias a sus hijos izquierdo y derecho dentro del árbol binario de búsqueda. Ambos nodos son la pieza mínima sobre la cual se construyen las estructuras más complejas del sistema.

#### 2.1.2 Lista Enlazada Simple

La lista enlazada simple es una estructura de datos lineal en la cual cada nodo contiene un enlace al siguiente nodo de la lista. En el sistema se utiliza para almacenar los hijos de cada categoría dentro del árbol n-ario (a través del TDA ListaHijos) y para almacenar los resultados de operaciones como el recorrido inorden de los libros o la recolección de libros ascendentes por ISBN (a través del TDA ListaLibros). La lista mantiene sus elementos ordenados alfabéticamente cuando corresponde, sin duplicados, y permite recorrer sus elementos en tiempo lineal.

#### 2.1.3 Árbol N-ario

El árbol n-ario es una estructura jerárquica en la cual cada nodo puede tener cualquier número de hijos. En el sistema, el árbol n-ario representa la organización completa del catálogo: la raíz es la categoría principal, y cada nodo puede contener tanto subcategorías como libros. Cada nodo del árbol mantiene una lista enlazada ordenada alfabéticamente con sus hijos, lo que permite mostrar la jerarquía en el orden correcto y realizar búsquedas recursivas en profundidad.

#### 2.1.4 Árbol Binario de Búsqueda

El árbol binario de búsqueda es una estructura en la cual cada nodo tiene como máximo dos hijos, y se cumple que todos los elementos del subárbol izquierdo son menores que la raíz, mientras que todos los elementos del subárbol derecho son mayores. En el sistema, cada categoría contiene un árbol binario de búsqueda indexado por ISBN, lo que permite insertar, buscar y eliminar libros en tiempo logarítmico. El recorrido inorden de este árbol produce los libros ordenados ascendentemente por ISBN de manera natural.

### 2.2 Modelado de la Estructura Jerárquica

La organización del catálogo se modela como un árbol n-ario en el cual cada nodo representa una categoría o subcategoría. Cada categoría posee un nombre único a nivel global, una lista enlazada de subcategorías ordenadas alfabéticamente y un árbol binario de búsqueda que contiene los libros asociados. Esta representación permite recorrer la jerarquía de manera recursiva y determinar rápidamente la ubicación de cualquier categoría o subcategoría, respetando la estructura de una biblioteca física organizada por secciones y estanterías.

### 2.3 Carga Incremental de Archivos XML

El sistema utiliza archivos en formato XML para la configuración inicial del catálogo. El lector de configuración procesa estos archivos y carga la información en las estructuras de datos correspondientes. El archivo es incremental, lo que significa que puede cargarse varias veces sobre el mismo sistema para agregar nuevas categorías y nuevos libros sin perder la información previamente registrada. El cargador soporta dos formatos de entrada: uno en el cual los nombres de las categorías y los ISBN se especifican como atributos XML, y otro en el cual dichos valores se especifican como texto interno de los nodos. Adicionalmente, el cargador implementa una estrategia de enlace diferido que permite resolver correctamente las categorías cuyos padres aparecen declarados después en el archivo.

### 2.4 Búsqueda y Organización de Libros

La búsqueda de libros es uno de los componentes más críticos del sistema. Se implementa aprovechando las propiedades del árbol binario de búsqueda: para buscar un libro por ISBN se desciende desde la raíz comparando el ISBN objetivo con el ISBN del nodo actual, avanzando hacia la izquierda o hacia la derecha según corresponda. Esta estrategia garantiza tiempos de búsqueda logarítmicos incluso cuando el catálogo contiene decenas de miles de libros. La búsqueda del libro con el menor ISBN consiste en descender siempre por el hijo izquierdo desde la raíz hasta encontrar una hoja; de manera análoga, el libro con el mayor ISBN se obtiene descendiendo siempre por el hijo derecho. El listado completo de libros en orden ascendente por ISBN se obtiene mediante un recorrido inorden de todos los árboles binarios del catálogo, seguido de un algoritmo MergeSort implementado sobre listas enlazadas para combinar los resultados parciales de cada categoría en una única secuencia global ordenada.

### 2.5 Eliminación de Libros

La eliminación de un libro se realiza sobre el árbol binario de búsqueda de la categoría correspondiente. El algoritmo considera los tres casos clásicos: eliminación de un nodo hoja, eliminación de un nodo con un único hijo y eliminación de un nodo con dos hijos. En este último caso, se reemplaza el dato del nodo a eliminar por el dato del sucesor inorden (el nodo con el menor ISBN del subárbol derecho) y posteriormente se elimina el sucesor original. Este procedimiento garantiza que la propiedad de orden del árbol binario de búsqueda se preserve tras la eliminación.

### 2.6 Generación de Reportes con Graphviz

El sistema genera reportes gráficos de la estructura del catálogo utilizando Graphviz. Se crean archivos en formato DOT que describen tanto el árbol n-ario de categorías como el árbol binario de búsqueda de libros de una categoría específica. Cada archivo DOT es procesado por la herramienta `dot` de Graphviz, que produce una imagen en formato PNG. Las imágenes resultantes se sirven a través del servidor web y se muestran al usuario en la interfaz gráfica, permitiéndole interpretar visualmente la organización completa del catálogo y las relaciones entre categorías y libros.

### 2.7 Interfaz Gráfica de Usuario

La interfaz se desarrolla como una aplicación web implementada con ASP.NET Core y JavaScript puro. La interfaz cuenta con una barra de navegación superior que permite acceder a cinco secciones: Inicio, Categorías, Libros, Cargar XML y Ayuda. La sección de Categorías permite visualizar la estructura jerárquica completa, crear nuevas categorías y generar el grafo correspondiente. La sección de Libros permite registrar nuevos libros, eliminar libros existentes, buscar libros por ISBN, obtener el libro con el menor y mayor ISBN, listar todos los libros en orden ascendente y visualizar el grafo del árbol binario de búsqueda de una categoría específica. La sección de Cargar XML permite subir cualquier archivo de configuración desde el equipo del usuario. La sección de Ayuda muestra la información del estudiante y el enlace a la documentación del proyecto en GitHub.

---

## 3. Conclusiones

El desarrollo del sistema de gestión de catálogo bibliográfico ha permitido cumplir con los objetivos planteados, demostrando la viabilidad de implementar una solución robusta utilizando exclusivamente Tipos de Datos Abstractos implementados desde cero.

1. La implementación de TDA propios (árbol n-ario, árbol binario de búsqueda y listas enlazadas simples) ha demostrado ser eficiente y funcional, permitiendo el almacenamiento y manipulación de la información sin necesidad de utilizar las colecciones nativas de C#. Esto evidencia una comprensión profunda de las estructuras de datos y su funcionamiento interno.

2. El uso de un árbol binario de búsqueda indexado por ISBN garantiza tiempos de búsqueda logarítmicos incluso cuando el catálogo contiene decenas de miles de libros. La búsqueda del libro con el menor y mayor ISBN se resuelve en tiempo proporcional a la altura del árbol, y el listado ascendente de todos los libros se obtiene mediante un recorrido inorden combinado con un algoritmo MergeSort implementado sobre listas enlazadas, sin recurrir a estructuras de ordenamiento nativas.

3. El árbol n-ario permite representar de manera natural la jerarquía de categorías y subcategorías del catálogo, respetando el orden alfabético solicitado y permitiendo tanto el recorrido completo desde la raíz como el recorrido a partir de una subcategoría específica. La lista enlazada de hijos mantiene el orden correcto y evita la duplicación de categorías a cualquier nivel.

4. La carga incremental de archivos XML y el soporte para dos formatos distintos de entrada (atributos y texto interno) ha sido correctamente implementada. La estrategia de enlace diferido permite resolver correctamente las categorías cuyos padres aparecen declarados después en el archivo, cumpliendo con uno de los requisitos fundamentales del proyecto.

5. La generación de reportes con Graphviz proporciona una representación visual clara tanto de la jerarquía de categorías como de la organización interna de los libros, facilitando la interpretación de los resultados y evidenciando la estructura completa del catálogo.

6. El sistema es capaz de determinar correctamente cuándo una operación es inválida, ya sea por ISBN duplicado, categoría inexistente, categoría con nombre duplicado o archivo XML mal formado, proporcionando retroalimentación clara al usuario a través de la interfaz gráfica.

---

## Referencias bibliográficas

Grupo Editorial RA-MA. (2021). C# y .NET 6: Curso práctico de programación. RA-MA Editorial.

Joyanes Aguilar, L. (2018). Estructuras de datos en C#. McGraw-Hill Education.

Microsoft Corporation. (2026). Documentación de C#. Recuperado de https://learn.microsoft.com/es-es/dotnet/csharp/

Galdámez, J. (2020). Programación orientada a objetos con C#. Editorial Universitaria, Universidad de San Carlos de Guatemala.

Myers, B. (2023). Graphviz: Graph Visualization Software. Recuperado de https://graphviz.org/documentation/

Sedgewick, R., & Wayne, K. (2011). Algorithms (4th ed.). Addison-Wesley.

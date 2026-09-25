// =========================================================
//  CONFIGURACIÓN
// =========================================================
const API = "";

// =========================================================
//  NAVEGACIÓN POR PESTAÑAS
// =========================================================
document.querySelectorAll(".tab").forEach(tab => {
    tab.addEventListener("click", () => {
        const destino = tab.dataset.tab;
        irA(destino);
    });
});

function irA(id) {
    document.querySelectorAll(".panel").forEach(p => p.classList.remove("activo"));
    document.querySelectorAll(".tab").forEach(t => t.classList.remove("activo"));

    const panel = document.getElementById(id);
    const tab = document.querySelector(`[data-tab="${id}"]`);

    if (panel) panel.classList.add("activo");
    if (tab) tab.classList.add("activo");
}

// =========================================================
//  TOAST (notificaciones flotantes)
// =========================================================
let toastTimeout = null;
function toast(mensaje, tipo = "info") {
    const el = document.getElementById("toast");
    el.textContent = mensaje;
    el.className = "toast visible " + tipo;

    if (toastTimeout) clearTimeout(toastTimeout);
    toastTimeout = setTimeout(() => {
        el.classList.remove("visible");
    }, 3500);
}

// =========================================================
//  UTILIDADES
// =========================================================

// Normaliza la URL de una imagen que viene del backend.
// Acepta "/Reportes/x.png", "\Reportes\x.png", "Reportes/x.png", etc.
function normalizarUrlImagen(ruta) {
    if (!ruta) return "";
    let url = ruta.replace(/\\/g, "/");
    if (!url.startsWith("/")) url = "/" + url;
    return url;
}

// Crea un <img> que se auto-renderiza en un contenedor, con
// diagnóstico visible si falla la carga.
function mostrarImagenEnContenedor(contenedor, url, altTexto) {
    contenedor.innerHTML = '<p class="placeholder">Cargando imagen...</p>';

    const urlFinal = normalizarUrlImagen(url) + "?t=" + Date.now();
    console.log(">>> Cargando imagen:", urlFinal);

    const img = new Image();
    img.alt = altTexto;
    img.style.maxWidth = "100%";
    img.style.height = "auto";
    img.style.display = "block";
    img.style.margin = "0 auto";

    img.onload = function () {
        console.log(">>> Imagen cargada OK:", urlFinal);
        contenedor.innerHTML = "";
        contenedor.appendChild(img);
    };

    img.onerror = function () {
        console.log(">>> ERROR cargando imagen:", urlFinal);
        contenedor.innerHTML =
            '<p class="placeholder" style="color:#dc2626; text-align:center;">' +
            'No se pudo cargar la imagen.<br>' +
            'Prueba abrir: <a href="' + urlFinal + '" target="_blank">' + urlFinal + '</a>' +
            '</p>';
    };

    img.src = urlFinal;
}

// =========================================================
//  API: SISTEMA
// =========================================================
async function inicializarSistema() {
    try {
        const res = await fetch(`${API}/api/sistema/inicializar`, { method: "POST" });
        const data = await res.json();
        document.getElementById("estadoSistema").textContent = "INICIALIZADO PAPA";
        document.getElementById("totalLibros").textContent = "0";
        document.getElementById("totalCategorias").textContent = "0";
        toast(data.msg || "Sistema reiniciado", "exito");
        cargarCategorias();
        cargarLibros();
    } catch (e) {
        toast("Error al inicializar: " + e.message, "error");
    }
}

// Subir archivo XML desde el navegador
async function subirXml() {
    const input = document.getElementById("archivoXml");
    const log = document.getElementById("logXml");

    if (!input.files || input.files.length === 0) {
        toast("Selecciona un archivo XML primero", "error");
        return;
    }

    const archivo = input.files[0];

    if (!archivo.name.toLowerCase().endsWith(".xml")) {
        toast("El archivo debe ser .xml", "error");
        return;
    }

    log.textContent = "Subiendo " + archivo.name + "...";

    const formData = new FormData();
    formData.append("archivo", archivo);

    try {
        const res = await fetch(`${API}/api/sistema/subir-xml`, {
            method: "POST",
            body: formData
        });
        const data = await res.json();

        log.textContent = data.msg || "Procesado";
        toast(data.msg || "XML cargado", data.ok ? "exito" : "error");


        cargarCategorias();
        cargarLibros();


        try {
            const rCats = await fetch(`${API}/api/categorias`);
            const dCats = await rCats.json();
            document.getElementById("totalCategorias").textContent =
                (dCats && dCats.length) ? dCats.length : 0;

            const rLibs = await fetch(`${API}/api/libros`);
            const dLibs = await rLibs.json();
            document.getElementById("totalLibros").textContent =
                (dLibs && dLibs.length) ? dLibs.length : 0;
        } catch (e) { /* silencioso */ }

    } catch (e) {
        log.textContent = "Error: " + e.message;
        toast("Error al subir XML", "error");
    }
}


async function cargarCategorias() {
    try {
        const res = await fetch(`${API}/api/categorias`);
        const data = await res.json();
        renderizarArbolCategorias(data);
    } catch (e) {
        toast("Error al cargar categorías", "error");
    }
}

function renderizarArbolCategorias(lista) {
    const cont = document.getElementById("arbolCategorias");

    if (!lista || lista.length === 0) {
        cont.innerHTML = '<p class="placeholder">No hay categorías registradas</p>';
        document.getElementById("totalCategorias").textContent = "0";
        return;
    }

    let html = "";
    for (let i = 0; i < lista.length; i++) {
        const cat = lista[i];
        const indent = "&nbsp;&nbsp;&nbsp;&nbsp;".repeat(cat.nivel);
        const clase = cat.nivel === 0 ? "nodo-cat raiz" : "nodo-cat";
        html += `<div class="${clase}">${indent}📁 ${cat.nombre}</div>`;
    }
    cont.innerHTML = html;
    document.getElementById("totalCategorias").textContent = lista.length;
}

async function crearCategoria() {
    const nombre = document.getElementById("catNombre").value.trim();
    const padre = document.getElementById("catPadre").value.trim();

    if (!nombre) {
        toast("Ingresa el nombre de la categoría", "error");
        return;
    }

    try {
        const res = await fetch(`${API}/api/categorias`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ nombre: nombre, padre: padre || null })
        });
        const data = await res.json();

        if (data.ok) {
            toast("Categoría creada", "exito");
            document.getElementById("catNombre").value = "";
            document.getElementById("catPadre").value = "";
            cargarCategorias();
        } else {
            toast(data.msg || "No se pudo crear", "error");
        }
    } catch (e) {
        toast("Error: " + e.message, "error");
    }
}


async function verGrafoCategorias() {
    const cont = document.getElementById("imgGrafoCats");

    try {
        const res = await fetch(`${API}/api/categorias/grafo`);
        const data = await res.json();

        if (data.imagen) {
            mostrarImagenEnContenedor(cont, data.imagen, "Grafo de categorías");
        } else {
            cont.innerHTML = '<p class="placeholder">No se pudo generar el grafo</p>';
        }
    } catch (e) {
        cont.innerHTML = '<p class="placeholder">Error: ' + e.message + '</p>';
    }
}


async function cargarLibros() {
    try {
        const res = await fetch(`${API}/api/libros`);
        const data = await res.json();
        renderizarTablaLibros(data);
    } catch (e) {
        toast("Error al cargar libros", "error");
    }
}

function renderizarTablaLibros(lista) {
    const tbody = document.getElementById("tbodyLibros");

    if (!lista || lista.length === 0) {
        tbody.innerHTML = '<tr><td colspan="5" class="vacio">No hay libros cargados</td></tr>';
        document.getElementById("totalLibros").textContent = "0";
        return;
    }

    let html = "";
    for (let i = 0; i < lista.length; i++) {
        const l = lista[i];
        html += `<tr>
            <td><strong>${l.isbn}</strong></td>
            <td>${l.titulo}</td>
            <td>${l.autor}</td>
            <td>${l.categoria}</td>
            <td>
                <button class="btn-peligro" onclick="eliminarLibro(${l.isbn})">
                    🗑️ Eliminar
                </button>
            </td>
        </tr>`;
    }
    tbody.innerHTML = html;
    document.getElementById("totalLibros").textContent = lista.length;
}

async function crearLibro() {
    const isbn = parseInt(document.getElementById("libIsbn").value);
    const titulo = document.getElementById("libTitulo").value.trim();
    const autor = document.getElementById("libAutor").value.trim();
    const categoria = document.getElementById("libCategoria").value.trim();

    if (!isbn || !titulo || !autor || !categoria) {
        toast("Completa todos los campos", "error");
        return;
    }

    try {
        const res = await fetch(`${API}/api/libros`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                isbn: isbn,
                titulo: titulo,
                autor: autor,
                categoria: categoria
            })
        });
        const data = await res.json();

        if (data.ok) {
            toast("Libro registrado", "exito");
            document.getElementById("libIsbn").value = "";
            document.getElementById("libTitulo").value = "";
            document.getElementById("libAutor").value = "";
            document.getElementById("libCategoria").value = "";
            cargarLibros();
        } else {
            toast(data.msg || "No se pudo registrar", "error");
        }
    } catch (e) {
        toast("Error: " + e.message, "error");
    }
}

async function eliminarLibro(isbn) {
    if (!confirm(`¿Eliminar el libro con ISBN ${isbn}?`)) return;

    try {
        const res = await fetch(`${API}/api/libros/${isbn}`, { method: "DELETE" });
        const data = await res.json();

        if (data.ok) {
            toast("Libro eliminado", "exito");
            cargarLibros();
        } else {
            toast(data.msg || "No se pudo eliminar", "error");
        }
    } catch (e) {
        toast("Error: " + e.message, "error");
    }
}

async function verMenor() {
    try {
        const res = await fetch(`${API}/api/libros/menor`);
        const data = await res.json();

        if (data.error) {
            toast(data.error, "error");
            return;
        }
        mostrarResultadoBusqueda(data, "Menor ISBN");
        toast(`Menor ISBN: ${data.isbn}`, "exito");
    } catch (e) {
        toast("Error: " + e.message, "error");
    }
}

async function verMayor() {
    try {
        const res = await fetch(`${API}/api/libros/mayor`);
        const data = await res.json();

        if (data.error) {
            toast(data.error, "error");
            return;
        }
        mostrarResultadoBusqueda(data, "Mayor ISBN");
        toast(`Mayor ISBN: ${data.isbn}`, "exito");
    } catch (e) {
        toast("Error: " + e.message, "error");
    }
}

async function buscarLibro() {
    const isbn = parseInt(document.getElementById("buscarIsbn").value);
    if (!isbn) {
        toast("Ingresa un ISBN", "error");
        return;
    }

    try {
        const res = await fetch(`${API}/api/libros/${isbn}`);
        const data = await res.json();

        if (data.error) {
            toast("Libro no encontrado", "error");
            document.getElementById("resultadoBusqueda").innerHTML = "";
            return;
        }
        mostrarResultadoBusqueda(data, "Resultado de búsqueda");
    } catch (e) {
        toast("Error: " + e.message, "error");
    }
}

function mostrarResultadoBusqueda(libro, titulo) {
    const cont = document.getElementById("resultadoBusqueda");
    cont.innerHTML = `
        <div class="card-libro">
            <h4>📖 ${titulo}</h4>
            <p><strong>ISBN:</strong> ${libro.isbn}</p>
            <p><strong>Título:</strong> ${libro.titulo}</p>
            <p><strong>Autor:</strong> ${libro.autor}</p>
            <p><strong>Categoría:</strong> ${libro.categoria}</p>
        </div>
    `;
}


async function verLibrosCategoria() {
    const nombre = document.getElementById("catFiltro").value.trim();
    if (!nombre) {
        toast("Ingresa el nombre de la categoría", "error");
        return;
    }

    const cont = document.getElementById("imgGrafoLibros");

    try {
        const res = await fetch(`${API}/api/libros/categoria/${encodeURIComponent(nombre)}`);
        const data = await res.json();

        if (data.imagen) {
            mostrarImagenEnContenedor(cont, data.imagen, "Grafo BST de libros");
        } else {
            cont.innerHTML = '<p class="placeholder">No se pudo generar el grafo</p>';
        }
    } catch (e) {
        cont.innerHTML = '<p class="placeholder">Error: ' + e.message + '</p>';
    }
}


window.addEventListener("DOMContentLoaded", () => {
    console.log("Sistema de Librería listo");
});

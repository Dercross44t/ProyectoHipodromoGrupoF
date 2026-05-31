using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;
using ProyectoHipodromoGrupoF.Modelo;
using System;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    [Authorize(Roles = "2")]
    public class PersonasController : Controller
    {
        private readonly UsuarioService     _usuarioService;
        private readonly PersonasService    _personasService;
        private readonly VeterinarioService _veterinarioService;
        private readonly CatalogosService   _catalogosService;

        public PersonasController(
            UsuarioService usuarioService,
            PersonasService personasService,
            VeterinarioService veterinarioService,
            CatalogosService catalogosService)
        {
            _usuarioService     = usuarioService;
            _personasService    = personasService;
            _veterinarioService = veterinarioService;
            _catalogosService   = catalogosService;
        }

        // ─── USUARIOS ────────────────────────────────────────────────────────────

        public IActionResult Index()
        {
            ViewBag.Personas = _personasService.ListarPersonas();
            ViewBag.Roles    = _catalogosService.ListarRoles();
            ViewBag.Provincias = _catalogosService.ListarProvincias();
            return View(_usuarioService.ListarUsuarios());
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult InsertarUsuario(Usuario usuario, Persona persona)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario.Contrasena))
                { TempData["Error"] = "La contraseña es obligatoria."; return RedirectToAction("Index"); }

                var existente = _personasService.ObtenerPersonaPorCedula(usuario.Cedula);
                if (existente == null)
                {
                    persona.Cedula = usuario.Cedula;
                    _personasService.InsertarPersona(persona);
                }

                _usuarioService.InsertarUsuario(usuario);
                
                // Sincronización automática de roles para evitar FK constraints (Ej: Caballo requiere Propietario)
                if (usuario.IdCatRol == 1)
                {
                    var propExistente = _personasService.ListarPropietarios().Find(p => p.Cedula == usuario.Cedula);
                    if (propExistente == null)
                    {
                        _personasService.InsertarPropietario(new Propietario { Cedula = usuario.Cedula, DescuentoProximaFacturacion = false });
                    }
                }
                else if (usuario.IdCatRol == 3)
                {
                    var encExistente = _personasService.ListarEncargados().Find(e => e.Cedula == usuario.Cedula);
                    if (encExistente == null)
                    {
                        _personasService.InsertarEncargado(new EncargadoEstablo { Cedula = usuario.Cedula });
                    }
                }

                TempData["Exito"] = "Usuario creado correctamente.";
            }
            catch (Exception ex) { TempData["Error"] = $"Error al registrar usuario: {ex.Message}"; }
            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult ActualizarUsuario(Usuario usuario)
        {
            try { _usuarioService.ActualizarUsuario(usuario); TempData["Exito"] = "Usuario actualizado."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult EliminarUsuario(string codigo)
        {
            try { _usuarioService.EliminarUsuario(codigo); TempData["Exito"] = "Usuario eliminado."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Index");
        }

        // ─── PERSONAS ────────────────────────────────────────────────────────────

        public IActionResult Personas()
        {
            ViewBag.Provincias = _catalogosService.ListarProvincias();
            ViewBag.TiposTelefono = _catalogosService.ListarTiposTelefono();
            ViewBag.TiposCorreo   = _catalogosService.ListarTiposCorreo();
            return View(_personasService.ListarPersonas());
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult InsertarPersona(
            Persona persona,
            [FromForm] string[] telefonos,
            [FromForm] int[]    tiposTelefono,
            [FromForm] string[] correos,
            [FromForm] int[]    tiposCorreo)
        {
            try
            {
                _personasService.InsertarPersona(persona);
                // Insertar teléfonos
                for (int i = 0; i < telefonos.Length; i++)
                    if (!string.IsNullOrWhiteSpace(telefonos[i]))
                        _personasService.InsertarTelefonoPersona(persona.Cedula, telefonos[i], tiposTelefono[i]);
                // Insertar correos
                for (int i = 0; i < correos.Length; i++)
                    if (!string.IsNullOrWhiteSpace(correos[i]))
                        _personasService.InsertarCorreoPersona(persona.Cedula, correos[i], tiposCorreo[i]);
                TempData["Exito"] = "Persona registrada correctamente.";
            }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Personas");
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult ActualizarPersona(Persona persona)
        {
            try { _personasService.ActualizarPersona(persona); TempData["Exito"] = "Persona actualizada."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Personas");
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult EliminarPersona(string cedula)
        {
            try { _personasService.EliminarPersona(cedula); TempData["Exito"] = "Persona eliminada."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Personas");
        }

        // ─── CASCADA GEOGRAFÍA (JSON para AJAX) ──────────────────────────────────

        [HttpGet]
        public IActionResult GetCantones(int idProvincia) =>
            Json(_catalogosService.ListarCantonesPorProvincia(idProvincia));

        [HttpGet]
        public IActionResult GetDistritos(int idCanton) =>
            Json(_catalogosService.ListarDistritosPorCanton(idCanton));

        [HttpGet]
        public IActionResult GetBarrios(int idDistrito) =>
            Json(_catalogosService.ListarBarriosPorDistrito(idDistrito));

        // ─── PROPIETARIOS ────────────────────────────────────────────────────────

        public IActionResult Propietarios()
        {
            var propietarios = _personasService.ListarPropietarios();
            var personas     = _personasService.ListarPersonas();
            var vista = from prop in propietarios
                        join per in personas on prop.Cedula equals per.Cedula
                        select new
                        {
                            Codigo    = prop.Codigo,
                            Cedula    = prop.Cedula,
                            Nombre    = $"{per.Nombre1} {per.Apellido1}".Trim(),
                            Descuento = prop.DescuentoProximaFacturacion
                        };
            ViewBag.Personas = personas;
            ViewBag.Provincias = _catalogosService.ListarProvincias();
            return View(vista);
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult InsertarPropietario(Propietario propietario, Persona persona)
        {
            try
            {
                // Verificamos si la persona ya existe
                var existente = _personasService.ObtenerPersonaPorCedula(propietario.Cedula);
                if (existente == null)
                {
                    // Si no existe, usamos los datos del form para crearla
                    persona.Cedula = propietario.Cedula;
                    _personasService.InsertarPersona(persona);
                }

                propietario.DescuentoProximaFacturacion = Request.Form["DescuentoProximaFacturacion"] == "true";
                _personasService.InsertarPropietario(propietario);
                TempData["Exito"] = "Propietario registrado exitosamente.";
            }
            catch (Exception ex) { TempData["Error"] = $"Error al registrar propietario: {ex.Message}"; }
            return RedirectToAction("Propietarios");
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult ActualizarPropietario(Propietario propietario)
        {
            try { _personasService.ActualizarPropietario(propietario); TempData["Exito"] = "Propietario actualizado."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Propietarios");
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult EliminarPropietario(string codigo)
        {
            try { _personasService.EliminarPropietario(codigo); TempData["Exito"] = "Propietario eliminado."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Propietarios");
        }

        // ─── VETERINARIOS ────────────────────────────────────────────────────────

        public IActionResult Veterinarios()
        {
            ViewBag.Personas = _personasService.ListarPersonas();
            return View(_veterinarioService.ListarVeterinarios());
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult InsertarVeterinario(Veterinario veterinario)
        {
            try { _veterinarioService.InsertarVeterinario(veterinario); TempData["Exito"] = "Veterinario registrado."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Veterinarios");
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult ActualizarVeterinario(Veterinario veterinario)
        {
            try { _veterinarioService.ActualizarVeterinario(veterinario); TempData["Exito"] = "Veterinario actualizado."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Veterinarios");
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult EliminarVeterinario(string codigo)
        {
            try { _veterinarioService.EliminarVeterinario(codigo); TempData["Exito"] = "Veterinario eliminado."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Veterinarios");
        }

        // ─── ENCARGADOS ───────────────────────────────────────────────────────────

        public IActionResult Encargados()
        {
            ViewBag.Personas = _personasService.ListarPersonas();
            return View(_personasService.ListarEncargados());
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult InsertarEncargado(EncargadoEstablo encargado)
        {
            try { _personasService.InsertarEncargado(encargado); TempData["Exito"] = "Encargado registrado."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Encargados");
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult EliminarEncargado(string codigo)
        {
            try { _personasService.EliminarEncargado(codigo); TempData["Exito"] = "Encargado eliminado."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Encargados");
        }
    }
}

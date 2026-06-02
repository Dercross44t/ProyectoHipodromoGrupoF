using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;
using ProyectoHipodromoGrupoF.Modelo;
using System;
using System.Linq;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    [Authorize(Roles = "2")]
    public class PersonasController : Controller
    {
        private readonly UsuarioService _usuarioService;
        private readonly PersonasService _personasService;
        private readonly VeterinarioService _veterinarioService;
        private readonly CatalogosService _catalogosService;

        public PersonasController(
            UsuarioService usuarioService,
            PersonasService personasService,
            VeterinarioService veterinarioService,
            CatalogosService catalogosService)
        {
            _usuarioService = usuarioService;
            _personasService = personasService;
            _veterinarioService = veterinarioService;
            _catalogosService = catalogosService;
        }

        private string ObtenerUsuarioActual()
        {
            return User.Identity?.Name ?? "usuario_desconocido";
        }

        // ─── USUARIOS ────────────────────────────────────────────────────────────

        public IActionResult Index()
        {
            ViewBag.Personas = _personasService.ListarPersonas();
            ViewBag.PersonasConRol = _personasService.ListarPersonasConRol();
            ViewBag.Roles = _catalogosService.ListarRoles();
            ViewBag.Provincias = _catalogosService.ListarProvincias();
            return View(_usuarioService.ListarUsuarios());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarUsuario(Usuario usuario)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario.Cedula))
                {
                    TempData["Error"] = "Debe seleccionar una persona.";
                    return RedirectToAction("Index");
                }

                var usuarioExistente = _usuarioService.ListarUsuarios()
                    .FirstOrDefault(u => u.Cedula == usuario.Cedula);

                if (usuarioExistente != null)
                {
                    TempData["Error"] = "Esta persona ya tiene un usuario registrado en el sistema.";
                    return RedirectToAction("Index");
                }

                if (string.IsNullOrWhiteSpace(usuario.Contrasena))
                {
                    TempData["Error"] = "La contraseña es obligatoria.";
                    return RedirectToAction("Index");
                }

                var personaConRol = _personasService.ListarPersonasConRol()
                    .FirstOrDefault(p => p.Cedula == usuario.Cedula);

                if (personaConRol == null)
                {
                    TempData["Error"] = "La persona seleccionada no tiene un rol definido.";
                    return RedirectToAction("Index");
                }

                usuario.Nombre = personaConRol.NombreUsuario;
                usuario.IdCatRol = personaConRol.IdCatRol;

                var usuarioActual = User.Identity?.Name ?? "usuario_desconocido";

                _usuarioService.InsertarUsuario(usuario, usuarioActual);

                TempData["Exito"] = $"Usuario \"{usuario.Nombre}\" creado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al registrar usuario: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarUsuario(Usuario usuario)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                _usuarioService.ActualizarUsuario(usuario, usuarioActual);
                TempData["Exito"] = "Usuario actualizado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarUsuario(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    TempData["Error"] = "Debe seleccionar un usuario para eliminar.";
                    return RedirectToAction("Index");
                }

                var usuarioActual = ObtenerUsuarioActual();
                _usuarioService.EliminarUsuario(codigo, usuarioActual);
                TempData["Exito"] = "Usuario eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // ─── PERSONAS ────────────────────────────────────────────────────────────

        public IActionResult Personas()
        {
            ViewBag.Provincias = _catalogosService.ListarProvincias();
            ViewBag.TiposTelefono = _catalogosService.ListarTiposTelefono();
            ViewBag.TiposCorreo = _catalogosService.ListarTiposCorreo();
            return View(_personasService.ListarPersonas());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarPersona(
            Persona persona,
            [FromForm] string[] telefonos,
            [FromForm] int[] tiposTelefono,
            [FromForm] string[] correos,
            [FromForm] int[] tiposCorreo)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                _personasService.InsertarPersona(persona, usuarioActual);

                for (int i = 0; i < telefonos.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(telefonos[i]))
                    {
                        _personasService.InsertarTelefonoPersona(persona.Cedula, telefonos[i], tiposTelefono[i]);
                    }
                }

                for (int i = 0; i < correos.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(correos[i]))
                    {
                        _personasService.InsertarCorreoPersona(persona.Cedula, correos[i], tiposCorreo[i]);
                    }
                }

                TempData["Exito"] = $"Persona \"{persona.Nombre1}\" registrada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Personas");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarPersona(Persona persona)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                _personasService.ActualizarPersona(persona, usuarioActual);
                TempData["Exito"] = "Persona actualizada.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Personas");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarPersona(string cedula)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cedula))
                {
                    TempData["Error"] = "Debe seleccionar una persona para eliminar.";
                    return RedirectToAction("Personas");
                }

                var usuarioActual = ObtenerUsuarioActual();
                _personasService.EliminarPersona(cedula, usuarioActual);
                TempData["Exito"] = "Persona eliminada.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Personas");
        }

        // ─── CASCADA GEOGRÁFICA ──────────────────────────────────────────────────

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
            var personas = _personasService.ListarPersonas();

            var vista = from propietario in propietarios
                        join persona in personas on propietario.Cedula equals persona.Cedula
                        select new
                        {
                            Codigo = propietario.Codigo,
                            Cedula = propietario.Cedula,
                            Nombre = $"{persona.Nombre1} {persona.Apellido1}".Trim(),
                            Descuento = propietario.DescuentoProximaFacturacion
                        };

            ViewBag.Personas = personas;
            ViewBag.PersonasSinRol = _personasService.ListarPersonasSinRol();
            return View(vista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarPropietario(Propietario propietario, Persona persona)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();

                var personaExistente = _personasService.ObtenerPersonaPorCedula(propietario.Cedula);
                if (personaExistente == null)
                {
                    persona.Cedula = propietario.Cedula;
                    _personasService.InsertarPersona(persona, usuarioActual);
                }

                propietario.DescuentoProximaFacturacion = Request.Form["DescuentoProximaFacturacion"] == "true";
                _personasService.InsertarPropietario(propietario, usuarioActual);

                TempData["Exito"] = "Propietario registrado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al registrar propietario: {ex.Message}";
            }

            return RedirectToAction("Propietarios");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarPropietario(Propietario propietario)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                _personasService.ActualizarPropietario(propietario, usuarioActual);
                TempData["Exito"] = "Propietario actualizado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Propietarios");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarPropietario(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    TempData["Error"] = "Debe seleccionar un propietario para eliminar.";
                    return RedirectToAction("Propietarios");
                }

                var usuarioActual = ObtenerUsuarioActual();
                _personasService.EliminarPropietario(codigo, usuarioActual);
                TempData["Exito"] = "Propietario eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Propietarios");
        }

        // ─── ADMINISTRADORES ─────────────────────────────────────────────────────

        public IActionResult Administradores()
        {
            ViewBag.Personas = _personasService.ListarPersonas();
            ViewBag.PersonasSinRol = _personasService.ListarPersonasSinRol();

            return View(_personasService.ListarAdministradores());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarAdministrador(Administrador administrador)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                _personasService.InsertarAdministrador(administrador, usuarioActual);
                TempData["Exito"] = "Administrador registrado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Administradores");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarAdministrador(Administrador administrador)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                _personasService.ActualizarAdministrador(administrador, usuarioActual);
                TempData["Exito"] = "Administrador actualizado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Administradores");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarAdministrador(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    TempData["Error"] = "Debe seleccionar un administrador para eliminar.";
                    return RedirectToAction("Administradores");
                }

                var usuarioActual = ObtenerUsuarioActual();
                _personasService.EliminarAdministrador(codigo, usuarioActual);
                TempData["Exito"] = "Administrador eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Administradores");
        }

        // ─── VETERINARIOS ────────────────────────────────────────────────────────

        public IActionResult Veterinarios()
        {
            ViewBag.Personas = _personasService.ListarPersonas();
            ViewBag.PersonasSinRol = _personasService.ListarPersonasSinRol();
            return View(_veterinarioService.ListarVeterinarios());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarVeterinario(Veterinario veterinario)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                _veterinarioService.InsertarVeterinario(veterinario, usuarioActual);
                TempData["Exito"] = "Veterinario registrado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Veterinarios");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarVeterinario(Veterinario veterinario)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                _veterinarioService.ActualizarVeterinario(veterinario, usuarioActual);
                TempData["Exito"] = "Veterinario actualizado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Veterinarios");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarVeterinario(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    TempData["Error"] = "Debe seleccionar un veterinario para eliminar.";
                    return RedirectToAction("Veterinarios");
                }

                var usuarioActual = ObtenerUsuarioActual();
                _veterinarioService.EliminarVeterinario(codigo, usuarioActual);
                TempData["Exito"] = "Veterinario eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Veterinarios");
        }

        // ─── ENCARGADOS DE ESTABLO ───────────────────────────────────────────────

        public IActionResult Encargados()
        {
            ViewBag.Personas = _personasService.ListarPersonas();
            ViewBag.PersonasSinRol = _personasService.ListarPersonasSinRol();
            return View(_personasService.ListarEncargados());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarEncargado(EncargadoEstablo encargado)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                _personasService.InsertarEncargado(encargado, usuarioActual);
                TempData["Exito"] = "Encargado registrado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Encargados");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarEncargado(EncargadoEstablo encargado)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                _personasService.ActualizarEncargado(encargado, usuarioActual);
                TempData["Exito"] = "Encargado actualizado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Encargados");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarEncargado(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    TempData["Error"] = "Debe seleccionar un encargado para eliminar.";
                    return RedirectToAction("Encargados");
                }

                var usuarioActual = ObtenerUsuarioActual();
                _personasService.EliminarEncargado(codigo, usuarioActual);
                TempData["Exito"] = "Encargado eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Encargados");
        }
    }
}

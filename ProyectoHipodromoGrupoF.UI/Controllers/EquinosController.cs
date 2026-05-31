using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;
using ProyectoHipodromoGrupoF.Modelo;
using System;
using System.Diagnostics;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    [Authorize(Roles = "1,2,3,4")]
    public class EquinosController : Controller
    {
        private readonly EquinosService     _equinosService;
        private readonly VeterinarioService _veterinarioService;
        private readonly PersonasService    _personasService;
        private readonly CatalogosService   _catalogosService;

        public EquinosController(
            EquinosService equinosService,
            VeterinarioService veterinarioService,
            PersonasService personasService,
            CatalogosService catalogosService)
        {
            _equinosService     = equinosService;
            _veterinarioService = veterinarioService;
            _personasService    = personasService;
            _catalogosService   = catalogosService;
        }

        // ─── HELPER: cargar catálogos en ViewBag ──────────────────────────────────

        private void CargarCatalogosCaballo()
        {
            ViewBag.Sexos      = _catalogosService.ListarSexos();
            ViewBag.Razas      = _catalogosService.ListarRazas();
            ViewBag.Estados    = _catalogosService.ListarEstadosCaballo();
            ViewBag.Propietarios = _personasService.ListarPropietarios();
            ViewBag.PersonasMap  = _personasService.ListarPersonas(); // Para mapear cédula a nombre
            ViewBag.Establos   = _equinosService.ListarEstablos();
            // Propietario actual (para rol 1)
            ViewBag.CodigoPropietarioActual = User.FindFirst("CodigoPropietario")?.Value ?? string.Empty;
        }

        // ─── CABALLOS ────────────────────────────────────────────────────────────

        [Authorize(Roles = "1,2,3,4")]
        public IActionResult Index()
        {
            CargarCatalogosCaballo();
            var alertas = new List<AlertaVeterinaria>();
            try { alertas = _veterinarioService.ObtenerAlertasCertificacion(); } catch { }
            ViewBag.AlertasVet = alertas.Count;

            if (User.IsInRole("1"))
            {
                var codigoPropietario = User.FindFirst("CodigoPropietario")?.Value ?? string.Empty;
                return View(_equinosService.ListarCaballosPorPropietario(codigoPropietario));
            }
            return View(_equinosService.ListarCaballos());
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public IActionResult InsertarCaballo(Caballo caballo)
        {
            try
            {
                // Rol 1 solo puede registrar con su propio código
                if (User.IsInRole("1"))
                    caballo.CodigoPropietario = User.FindFirst("CodigoPropietario")?.Value ?? caballo.CodigoPropietario;
                var usuarioActual = User.Identity?.Name ?? "usuario_desconocido";
                _equinosService.InsertarCaballo(caballo, usuarioActual);
                TempData["Exito"] = $"Caballo \"{caballo.Nombre}\" registrado correctamente.";
            }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public IActionResult ActualizarCaballo(Caballo caballo)
        {
            try
            {
                if (User.IsInRole("1"))
                    caballo.CodigoPropietario = User.FindFirst("CodigoPropietario")?.Value ?? caballo.CodigoPropietario;
                
                var existente = _equinosService.ListarCaballos().Find(c => c.Codigo == caballo.Codigo);
                if (existente != null)
                    caballo.IdCatEstadoCaballo = existente.IdCatEstadoCaballo;
                var usuarioActual = User.Identity?.Name ?? "usuario_desconocido";
                _equinosService.ActualizarCaballo(caballo, usuarioActual);
                TempData["Exito"] = "Caballo actualizado correctamente.";
            }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public IActionResult EliminarCaballo(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    TempData["Error"] = "Debe seleccionar un caballo para eliminar.";
                    return RedirectToAction("Index");
                }

                var usuarioActual = User.Identity?.Name ?? "usuario_desconocido";

                _equinosService.EliminarCaballo(codigo, usuarioActual);

                TempData["Exito"] = "Caballo eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // ─── ESTABLOS ────────────────────────────────────────────────────────────

        [Authorize(Roles = "2,3")]
        public IActionResult Establos()
        {
            ViewBag.EstadosEstablo = _catalogosService.ListarEstadosEstablo();
            ViewBag.Provincias     = _catalogosService.ListarProvincias();
            ViewBag.PersonasMap    = _personasService.ListarPersonas();
            ViewBag.Encargados     = _personasService.ListarEncargados();
            return View(_equinosService.ListarEstablos());
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2,3")]
        public IActionResult InsertarEstablo(Establo establo)
        {
            try { _equinosService.InsertarEstablo(establo); TempData["Exito"] = "Establo registrado."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Establos");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2,3")]
        public IActionResult ActualizarEstablo(Establo establo)
        {
            try { _equinosService.ActualizarEstablo(establo); TempData["Exito"] = "Establo actualizado."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Establos");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult EliminarEstablo(string codigo)
        {
            try { _equinosService.EliminarEstablo(codigo); TempData["Exito"] = "Establo eliminado."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Establos");
        }

        // ─── HISTORIAL VETERINARIO ───────────────────────────────────────────────

        [Authorize(Roles = "4")]
        public IActionResult HistorialVeterinario()
        {
            ViewBag.Caballos      = _equinosService.ListarCaballos();
            ViewBag.Veterinarios  = _veterinarioService.ListarVeterinarios();
            ViewBag.FechaMinimaVencimiento = DateTime.Today.AddMonths(6).ToString("yyyy-MM-dd");
            return View(_veterinarioService.ListarHistorialVeterinario());
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "4")]
        public IActionResult InsertarHistorial(HistorialVeterinario historial)
        {
            try
            {
                if (historial.FechaVencimientoCertificacion < DateTime.Today.AddMonths(6))
                {
                    TempData["Error"] = "La fecha de vencimiento de la certificación debe ser mínimo 6 meses desde hoy.";
                    return RedirectToAction("HistorialVeterinario");
                }

                var usuarioActual = User.Identity?.Name ?? "usuario_desconocido";

                historial.CodigoVeterinario = _veterinarioService.ObtenerCodigoVeterinarioPorUsuario(usuarioActual);

                if (string.IsNullOrWhiteSpace(historial.CodigoVeterinario))
                {
                    TempData["Error"] = "No se encontró un veterinario asociado al usuario actual.";
                    return RedirectToAction("HistorialVeterinario");
                }

                _veterinarioService.InsertarHistorial(historial, usuarioActual);

                TempData["Exito"] = "Registro veterinario guardado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("HistorialVeterinario");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "4")]
        public IActionResult ActualizarHistorial(HistorialVeterinario historial)
        {
            try
            {
                var usuarioActual = User.Identity?.Name ?? "usuario_desconocido";

                _veterinarioService.ActualizarHistorial(historial, usuarioActual);

                TempData["Exito"] = $"Registro actualizado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "4")]
        public IActionResult EliminarHistorial(string codigo)
        {
                try
                {
                    if (string.IsNullOrWhiteSpace(codigo))
                    {
                        TempData["Error"] = "Debe seleccionar un historial para eliminar.";
                        return RedirectToAction("Index");
                    }

                    var usuarioActual = User.Identity?.Name ?? "usuario_desconocido";

                    _veterinarioService.EliminarHistorial(codigo, usuarioActual);

                    TempData["Exito"] = "Evento eliminado.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error: {ex.Message}";
                }

                return RedirectToAction("Index");
        }

        // ─── AJAX: verificar certificación ────────────────────────────────────────

        [HttpGet]
        public IActionResult TieneCertificacion(string codigoCaballo)
        {
            try
            {
                bool tiene = _veterinarioService.ObtenerAlertasCertificacion()
                             .TrueForAll(_ => true); // usa InscripcionesRepository
                // Más directo: consultar InscripcionesService
                var tiene2 = true;
                using var scope = HttpContext.RequestServices.CreateScope();
                var inscSvc = scope.ServiceProvider.GetRequiredService<InscripcionesService>();
                tiene2 = inscSvc.TieneCertificacionVigente(codigoCaballo);
                return Json(new { tieneCertificacion = tiene2 });
            }
            catch { return Json(new { tieneCertificacion = false }); }
        }
    }
}

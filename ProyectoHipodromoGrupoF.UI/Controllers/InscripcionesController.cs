using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;
using ProyectoHipodromoGrupoF.Modelo;
using System;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    [Authorize(Roles = "1,2,3")]
    public class InscripcionesController : Controller
    {
        private readonly InscripcionesService _inscripcionesService;
        private readonly EquinosService       _equinosService;
        private readonly EventosService       _eventosService;
        private readonly CatalogosService     _catalogosService;
        private readonly FacturacionService _facturacionService;

        public InscripcionesController(
            InscripcionesService inscripcionesService,
            EquinosService equinosService,
            EventosService eventosService,
            CatalogosService catalogosService,
            FacturacionService facturacionService)
        {
            _inscripcionesService = inscripcionesService;
            _equinosService       = equinosService;
            _eventosService       = eventosService;
            _catalogosService     = catalogosService;
            _facturacionService = facturacionService;
        }

        public IActionResult Index()
        {
            ViewBag.Eventos = _eventosService.ListarEventos();
            ViewBag.EstadosInscripcion = _catalogosService.ListarEstadosInscripcion();

            List<Caballo> caballos;

            if (User.IsInRole("1"))
            {
                var codigoPropietario = User.FindFirst("CodigoPropietario")?.Value ?? string.Empty;

                caballos = _equinosService.ListarCaballosPorPropietario(codigoPropietario);
                ViewBag.Caballos = caballos;

                var todosCaballosInscripcion = _inscripcionesService.ListarCaballosParaInscripcion();

                ViewBag.CaballosInscripcion = todosCaballosInscripcion
                    .Where(ci => caballos.Any(c => c.Codigo == ci.Codigo))
                    .ToList();

                var certsProp = new Dictionary<string, bool>();

                foreach (var c in caballos)
                    certsProp[c.Codigo] = _inscripcionesService.TieneCertificacionVigente(c.Codigo);

                ViewBag.Certificaciones = certsProp;

                return View(_inscripcionesService.ListarInscripcionesPorPropietario(codigoPropietario));
            }

            caballos = _equinosService.ListarCaballos();
            ViewBag.Caballos = caballos;
            ViewBag.CaballosInscripcion = _inscripcionesService.ListarCaballosParaInscripcion();

            var certs = new Dictionary<string, bool>();

            foreach (var c in caballos)
                certs[c.Codigo] = _inscripcionesService.TieneCertificacionVigente(c.Codigo);

            ViewBag.Certificaciones = certs;

            return View(_inscripcionesService.ListarInscripciones());
        }

        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult InsertarInscripcion(Inscripcion inscripcion)
        {
            try
            {
                inscripcion.IdCatEstadoInscripcion = 1; // Forzar a "Pendiente"
                inscripcion.Fecha = DateTime.Today;

                var caballos = _inscripcionesService.ListarCaballosParaInscripcion();
                var caballo = caballos.FirstOrDefault(c => c.Codigo == inscripcion.CodigoCaballo);

                if (caballo == null)
                {
                    TempData["Error"] = "El caballo seleccionado no existe.";
                    return RedirectToAction("Index");
                }

                if (!caballo.PuedeInscribirse)
                {
                    TempData["Error"] = $"El caballo no puede inscribirse: {caballo.Motivo}";
                    return RedirectToAction("Index");
                }

                _inscripcionesService.InsertarInscripcion(inscripcion);
                TempData["Exito"] = "Inscripción registrada correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                // Error de negocio: sin certificación veterinaria
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al registrar inscripción: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult ActualizarInscripcion(Inscripcion inscripcion)
        {
            try
            {
                _inscripcionesService.ActualizarInscripcion(inscripcion);

                if (inscripcion.IdCatEstadoInscripcion == 2) // 2 = Aprobada
                {
                    var inscripcionBD = _inscripcionesService.ListarInscripciones()
                        .FirstOrDefault(i => i.Codigo == inscripcion.Codigo);

                    if (inscripcionBD == null)
                        throw new Exception("No se encontró la inscripción aprobada.");

                    var caballo = _equinosService.ListarCaballos()
                        .FirstOrDefault(c => c.Codigo == inscripcionBD.CodigoCaballo);

                    if (caballo == null)
                        throw new Exception("No se encontró el caballo de la inscripción.");

                    var usuarioActual = User.Identity?.Name ?? "usuario_desconocido";

                    _facturacionService.GenerarFacturaPorInscripcion(
                        caballo.CodigoPropietario,
                        inscripcionBD.CodigoEvento,
                        usuarioActual
                    );
                }

                TempData["Exito"] = "Inscripción actualizada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult EliminarInscripcion(string codigo)
        {
            try
            {
                _inscripcionesService.EliminarInscripcion(codigo);
                TempData["Exito"] = "Inscripción eliminada.";
            }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "1,2")] // Propietarios y Admin
        public IActionResult CancelarInscripcion(string codigo)
        {
            try
            {
                var inscripcion = _inscripcionesService.ListarInscripciones().FirstOrDefault(i => i.Codigo == codigo);
                if (inscripcion != null && inscripcion.IdCatEstadoInscripcion == 1) // 1 = Pendiente
                {
                    inscripcion.IdCatEstadoInscripcion = 4; // 4 = Cancelada (3 es Rechazada)
                    _inscripcionesService.ActualizarInscripcion(inscripcion);
                    TempData["Exito"] = "La inscripción ha sido cancelada exitosamente.";
                }
                else
                {
                    TempData["Error"] = "No se puede cancelar una inscripción que no está pendiente o no existe.";
                }
            }
            catch (Exception ex) { TempData["Error"] = $"Error al cancelar: {ex.Message}"; }
            return RedirectToAction("Index");
        }

        // ─── AJAX: verificar certificación en tiempo real ─────────────────────────

        [HttpGet]
        public IActionResult VerificarCertificacion(string codigoCaballo)
        {
            var tiene = _inscripcionesService.TieneCertificacionVigente(codigoCaballo);
            return Json(new { tieneCertificacion = tiene });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;
using ProyectoHipodromoGrupoF.UI.Models;
using System.Diagnostics;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly VeterinarioService  _veterinarioService;
        private readonly EquinosService      _equinosService;
        private readonly EventosService      _eventosService;
        private readonly FacturacionService  _facturacionService;
        private readonly InscripcionesService _inscripcionesService;

        public HomeController(
            VeterinarioService veterinarioService,
            EquinosService equinosService,
            EventosService eventosService,
            FacturacionService facturacionService,
            InscripcionesService inscripcionesService)
        {
            _veterinarioService  = veterinarioService;
            _equinosService      = equinosService;
            _eventosService      = eventosService;
            _facturacionService  = facturacionService;
            _inscripcionesService = inscripcionesService;
        }

        public IActionResult Index()
        {
            // Propietarios van directo a sus caballos
            if (User.IsInRole("1"))
                return RedirectToAction("Index", "Equinos");
            // Veterinarios van al historial
            if (User.IsInRole("4"))
                return RedirectToAction("HistorialVeterinario", "Equinos");
            // Encargados de Establo van al inventario
            if (User.IsInRole("3"))
                return RedirectToAction("Index", "Inventario");

            // Administradores: Dashboard completo
            return RedirectToAction("Dashboard");
        }

        [Authorize(Roles = "2")]
        public IActionResult Dashboard()
        {
            try
            {
                var caballos           = _equinosService.ListarCaballos();
                var eventos            = _eventosService.ListarEventos();
                var alertas            = _veterinarioService.ObtenerAlertasCertificacion();
                var kpis               = _facturacionService.ObtenerKPIs();
                var inscripciones      = _inscripcionesService.ListarInscripciones();
                var pendientes         = inscripciones.FindAll(i => i.IdCatEstadoInscripcion == 1);

                ViewBag.TotalCaballos      = caballos.Count;
                ViewBag.EventosProgramados = eventos.FindAll(e => e.IdCatEstadoEvento == 1).Count;
                ViewBag.AlertasVet         = alertas.Count;
                ViewBag.AlertasDetalle     = alertas;
                ViewBag.CuentasPorCobrar   = kpis.CuentasPorCobrar;
                ViewBag.IngresosMes        = kpis.IngresosMes;
                ViewBag.FacturasPagadas    = kpis.FacturasPagadas;
                ViewBag.FacturasPendientes = kpis.FacturasPendientes;
                ViewBag.InscripcionesPend  = pendientes.Count;
                ViewBag.Eventos            = eventos; // Para selector de premios
            }
            catch
            {
                ViewBag.TotalCaballos      = 0;
                ViewBag.EventosProgramados = 0;
                ViewBag.AlertasVet         = 0;
                ViewBag.AlertasDetalle     = new List<ProyectoHipodromoGrupoF.Modelo.AlertaVeterinaria>();
                ViewBag.CuentasPorCobrar   = 0;
                ViewBag.IngresosMes        = 0;
                ViewBag.FacturasPagadas    = 0;
                ViewBag.FacturasPendientes = 0;
                ViewBag.InscripcionesPend  = 0;
                ViewBag.Eventos            = new List<ProyectoHipodromoGrupoF.Modelo.Evento>();
            }

            return View();
        }

        [Authorize(Roles = "2")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EjecutarEvaluarPropietarios()
        {
            try
            {
                _facturacionService.EjecutarEvaluacionPropietariosFrecuentes();
                TempData["Exito"] = "Evaluación de propietarios frecuentes ejecutada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al evaluar propietarios: {ex.Message}";
            }
            return RedirectToAction("Dashboard");
        }

        [Authorize(Roles = "2")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CalcularPremiosEvento(string codigoEvento)
        {
            try
            {
                _facturacionService.CalcularPremiosEvento(codigoEvento);
                TempData["Exito"] = $"Premios del evento {codigoEvento} calculados (60%/25%/15%).";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al calcular premios: {ex.Message}";
            }
            return RedirectToAction("Dashboard");
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

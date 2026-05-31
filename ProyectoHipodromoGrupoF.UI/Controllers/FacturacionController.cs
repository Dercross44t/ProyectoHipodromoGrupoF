using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;
using ProyectoHipodromoGrupoF.Modelo;
using System;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    [Authorize(Roles = "1,2")]
    public class FacturacionController : Controller
    {
        private readonly FacturacionService _facturacionService;
        private readonly PersonasService    _personasService;
        private readonly EventosService     _eventosService;
        private readonly CatalogosService   _catalogosService;

        public FacturacionController(
            FacturacionService facturacionService,
            PersonasService personasService,
            EventosService eventosService,
            CatalogosService catalogosService)
        {
            _facturacionService = facturacionService;
            _personasService    = personasService;
            _eventosService     = eventosService;
            _catalogosService   = catalogosService;
        }

        // ─── FACTURAS ────────────────────────────────────────────────────────────

        [Authorize(Roles = "2")]
        public IActionResult Index()
        {
            ViewBag.EstadosPago  = _catalogosService.ListarEstadosPago();
            ViewBag.Propietarios = _personasService.ListarPropietarios();
            ViewBag.PersonasMap  = _personasService.ListarPersonas(); // para join de nombre
            ViewBag.Eventos      = _eventosService.ListarEventos();
            return View(_facturacionService.ListarFacturas());
        }

        [Authorize(Roles = "1")]
        public IActionResult MisFacturas()
        {
            var codigoPropietario = User.FindFirst("CodigoPropietario")?.Value ?? string.Empty;
            ViewBag.EstadosPago  = _catalogosService.ListarEstadosPago();
            ViewBag.Propietarios = _personasService.ListarPropietarios();
            ViewBag.PersonasMap  = _personasService.ListarPersonas();
            ViewBag.Eventos      = _eventosService.ListarEventos();
            return View("Index", _facturacionService.ListarFacturasPorPropietario(codigoPropietario));
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult InsertarFactura(Factura factura)
        {
            try
            {
                _facturacionService.InsertarFactura(factura);
                TempData["Exito"] = "Factura generada con IVA 13% aplicado.";
            }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult ActualizarFactura(Factura factura)
        {
            try { _facturacionService.ActualizarFactura(factura); TempData["Exito"] = "Factura actualizada."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult EliminarFactura(string codigo)
        {
            try { _facturacionService.EliminarFactura(codigo); TempData["Exito"] = "Factura eliminada."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult GenerarFacturaAutomatica(string codigoPropietario, string codigoEvento, double precioInscripcion)
        {
            try
            {
                _facturacionService.GenerarFacturaAutomatica(codigoPropietario, codigoEvento, precioInscripcion);
                TempData["Exito"] = "Factura generada automáticamente (IVA 13% + comisión 5%).";
            }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Index");
        }

        // ─── ENDPOINT AJAX: obtener descuento del propietario ─────────────────────

        [HttpGet]
        [Authorize(Roles = "2")]
        public IActionResult GetDescuentoPropietario(string codigoPropietario)
        {
            var prop = _personasService.ObtenerPropietario(codigoPropietario);
            return Json(new { tieneDescuento = prop?.DescuentoProximaFacturacion ?? false });
        }

        [HttpGet]
        [Authorize(Roles = "2")]
        public IActionResult GetTotalFactura(string codigoFactura)
        {
            var total = _facturacionService.ObtenerTotalFactura(codigoFactura);
            return Json(new { total });
        }

        // ─── TRANSACCIONES ────────────────────────────────────────────────────────

        [Authorize(Roles = "2")]
        public IActionResult Transacciones()
        {
            ViewBag.Facturas      = _facturacionService.ListarFacturas();
            ViewBag.MetodosPago   = _catalogosService.ListarMetodosPago();
            return View(_facturacionService.ListarTransacciones());
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult InsertarTransaccion(HistorialTransaccion transaccion)
        {
            try
            {
                _facturacionService.InsertarTransaccion(transaccion);
                TempData["Exito"] = "Transacción registrada correctamente.";
            }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Transacciones");
        }
    }
}

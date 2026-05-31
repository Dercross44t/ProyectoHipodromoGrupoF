using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;
using ProyectoHipodromoGrupoF.Modelo;
using System;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    [Authorize(Roles = "1,2")]
    public class EventosController : Controller
    {
        private readonly EventosService   _eventosService;
        private readonly CatalogosService _catalogosService;

        public EventosController(EventosService eventosService, CatalogosService catalogosService)
        {
            _eventosService   = eventosService;
            _catalogosService = catalogosService;
        }

        // ─── EVENTOS ─────────────────────────────────────────────────────────────

        public IActionResult Index()
        {
            ViewBag.EstadosEvento = _catalogosService.ListarEstadosEvento();
            ViewBag.Carreras      = _eventosService.ListarCarreras();
            return View(_eventosService.ListarEventos());
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult InsertarEvento(Evento evento)
        {
            try
            {
                evento.IdCatEstadoEvento = 1;

                var usuarioActual = User.Identity?.Name ?? "usuario_desconocido";

                _eventosService.InsertarEvento(evento, usuarioActual);

                TempData["Exito"] = $"Evento \"{evento.Nombre}\" registrado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult ActualizarEvento(Evento evento)
        {
            try
            {
                var usuarioActual = User.Identity?.Name ?? "usuario_desconocido";

                _eventosService.ActualizarEvento(evento, usuarioActual);

                TempData["Exito"] = $"Evento \"{evento.Nombre}\" registrado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult EliminarEvento(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                {
                    TempData["Error"] = "Debe seleccionar un caballo para eliminar.";
                    return RedirectToAction("Index");
                }

                var usuarioActual = User.Identity?.Name ?? "usuario_desconocido";

                _eventosService.EliminarEvento(codigo, usuarioActual);

                TempData["Exito"] = "Evento eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
            //try { _eventosService.EliminarEvento(codigo, usuarioActual); TempData["Exito"] = "Evento eliminado."; }
            //catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            //return RedirectToAction("Index");
        }

        // ─── CARRERAS ─────────────────────────────────────────────────────────────

        [Authorize(Roles = "2")]
        public IActionResult Carreras()
        {
            ViewBag.TiposCarrera = _catalogosService.ListarTiposCarrera();
            ViewBag.Distancias   = _catalogosService.ListarDistancias();
            return View(_eventosService.ListarCarreras());
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult InsertarCarrera(Carrera carrera)
        {
            try { _eventosService.InsertarCarrera(carrera); TempData["Exito"] = $"Carrera \"{carrera.Nombre}\" registrada."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Carreras");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult ActualizarCarrera(Carrera carrera)
        {
            try { _eventosService.ActualizarCarrera(carrera); TempData["Exito"] = "Carrera actualizada."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Carreras");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult EliminarCarrera(string codigo)
        {
            try { _eventosService.EliminarCarrera(codigo); TempData["Exito"] = "Carrera eliminada."; }
            catch (Exception ex) { TempData["Error"] = $"Error: {ex.Message}"; }
            return RedirectToAction("Carreras");
        }
    }
}

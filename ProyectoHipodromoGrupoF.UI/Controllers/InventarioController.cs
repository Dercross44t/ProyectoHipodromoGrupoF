using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;
using ProyectoHipodromoGrupoF.Modelo;
using System;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    [Authorize(Roles = "2,3")]
    public class InventarioController : Controller
    {
        private readonly InventarioService _inventarioService;
        private readonly EquinosService    _equinosService;

        public InventarioController(InventarioService inventarioService, EquinosService equinosService)
        {
            _inventarioService = inventarioService;
            _equinosService    = equinosService;
        }

        public IActionResult Index()
        {
            return View(_inventarioService.ListarSuministros());
        }

        public IActionResult Alimentacion()
        {
            ViewBag.Caballos     = _equinosService.ListarCaballos();
            ViewBag.Suministros  = _inventarioService.ListarSuministros();
            return View(_inventarioService.ListarAlimentacion());
        }

        // ─── SUMINISTROS ─────────────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarSuministro(Suministro suministro)
        {
            try
            {
                _inventarioService.InsertarSuministro(suministro);
                TempData["Exito"] = "Suministro registrado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al registrar suministro: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarSuministro(Suministro suministro)
        {
            try
            {
                _inventarioService.ActualizarSuministro(suministro);
                TempData["Exito"] = "Suministro actualizado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al actualizar suministro: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult EliminarSuministro(string codigo)
        {
            try
            {
                _inventarioService.EliminarSuministro(codigo);
                TempData["Exito"] = "Suministro eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar suministro: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        // ─── ALIMENTACIÓN ────────────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "3")]
        public IActionResult InsertarAlimentacion(Alimentacion alimentacion)
        {
            try
            {
                _inventarioService.InsertarAlimentacion(alimentacion);
                TempData["Exito"] = "Registro de alimentación guardado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al registrar alimentación: {ex.Message}";
            }
            return RedirectToAction("Alimentacion");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "3")]
        public IActionResult ActualizarAlimentacion(Alimentacion alimentacion)
        {
            try
            {
                _inventarioService.ActualizarAlimentacion(alimentacion);
                TempData["Exito"] = "Registro de alimentación actualizado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al actualizar alimentación: {ex.Message}";
            }
            return RedirectToAction("Alimentacion");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "3")]
        public IActionResult EliminarAlimentacion(string codigo)
        {
            try
            {
                _inventarioService.EliminarAlimentacion(codigo);
                TempData["Exito"] = "Registro eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar registro: {ex.Message}";
            }
            return RedirectToAction("Alimentacion");
        }
    }
}

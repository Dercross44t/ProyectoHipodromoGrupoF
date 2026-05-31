using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;
using ProyectoHipodromoGrupoF.Modelo;
using System;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    [Authorize(Roles = "2")] // Solo Administrador
    public class ProveedoresController : Controller
    {
        private readonly ProveedoresService _proveedoresService;

        public ProveedoresController(ProveedoresService proveedoresService)
        {
            _proveedoresService = proveedoresService;
        }

        public IActionResult Index()
        {
            return View(_proveedoresService.ListarProveedores());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarProveedor(Proveedor proveedor)
        {
            try
            {
                _proveedoresService.InsertarProveedor(proveedor);
                TempData["Exito"] = "Proveedor registrado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al registrar proveedor: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarProveedor(Proveedor proveedor)
        {
            try
            {
                _proveedoresService.ActualizarProveedor(proveedor);
                TempData["Exito"] = "Proveedor actualizado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al actualizar proveedor: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarProveedor(string codigo)
        {
            try
            {
                _proveedoresService.EliminarProveedor(codigo);
                TempData["Exito"] = "Proveedor eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar proveedor: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
    }
}

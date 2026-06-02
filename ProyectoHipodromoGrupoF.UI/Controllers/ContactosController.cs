using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;
using ProyectoHipodromoGrupoF.Modelo;
using System;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    [Authorize(Roles = "1,2,3,4")]
    public class ContactosController : Controller
    {
        private readonly ContactosService _contactosService;
        private readonly PersonasService _personasService;
        private readonly ProveedoresService _proveedoresService;
        private readonly CatalogosService _catalogosService;

        public ContactosController(
            ContactosService contactosService,
            PersonasService personasService,
            ProveedoresService proveedoresService,
            CatalogosService catalogosService)
        {
            _contactosService = contactosService;
            _personasService = personasService;
            _proveedoresService = proveedoresService;
            _catalogosService = catalogosService;
        }

        public IActionResult Index()
        {
            ViewBag.Personas = _personasService.ListarPersonas();
            ViewBag.Proveedores = _proveedoresService.ListarProveedores();
            ViewBag.TiposTelefono = _catalogosService.ListarTiposTelefono();
            ViewBag.TiposCorreo = _catalogosService.ListarTiposCorreo();

            var model = new ContactoViewModel
            {
                TelefonosPersonas = _contactosService.ListarTelefonosPersonas(),
                CorreosPersonas = _contactosService.ListarCorreosPersonas(),
                TelefonosProveedores = _contactosService.ListarTelefonosProveedores(),
                CorreosProveedores = _contactosService.ListarCorreosProveedores()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarTelefonoPersona(TelefonoPersona telefono)
        {
            try
            {
                _contactosService.InsertarTelefonoPersona(telefono);
                TempData["Exito"] = "Teléfono de persona registrado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarCorreoPersona(CorreoElectronicoPersona correo)
        {
            try
            {
                _contactosService.InsertarCorreoPersona(correo);
                TempData["Exito"] = "Correo de persona registrado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarTelefonoProveedor(TelefonoProveedor telefono)
        {
            try
            {
                _contactosService.InsertarTelefonoProveedor(telefono);
                TempData["Exito"] = "Teléfono de proveedor registrado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertarCorreoProveedor(CorreoElectronicoProveedor correo)
        {
            try
            {
                _contactosService.InsertarCorreoProveedor(correo);
                TempData["Exito"] = "Correo de proveedor registrado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
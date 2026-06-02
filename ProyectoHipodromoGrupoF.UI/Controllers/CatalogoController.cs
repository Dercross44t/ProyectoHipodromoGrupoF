using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    [Authorize(Roles = "2")]
    public class CatalogoController : Controller
    {
        private readonly CatalogoService _catalogoService;

        public CatalogoController(CatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        public IActionResult Index()
        {
            ViewBag.Bitacoras = _catalogoService.ListarBitacorasTrimestrales();
            return View();
        }
    }
}

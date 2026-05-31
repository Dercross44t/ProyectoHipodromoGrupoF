using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;
using ProyectoHipodromoGrupoF.Modelo;
using System;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    [Authorize(Roles = "1,2")]
    public class ResultadosController : Controller
    {
        private readonly ResultadosService _resultadosService;
        private readonly InscripcionesService _inscripcionesService;
        private readonly EquinosService _equinosService;
        private readonly EventosService _eventosService;

        public ResultadosController(
            ResultadosService resultadosService,
            InscripcionesService inscripcionesService,
            EquinosService equinosService,
            EventosService eventosService)
        {
            _resultadosService = resultadosService;
            _inscripcionesService = inscripcionesService;
            _equinosService = equinosService;
            _eventosService = eventosService;
        }

        public IActionResult Index()
        {
            ViewBag.Inscripciones = _inscripcionesService.ListarInscripciones();
            ViewBag.Caballos = _equinosService.ListarCaballos();
            ViewBag.Eventos = _eventosService.ListarEventos();
            return View(_resultadosService.ListarResultados());
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult InsertarResultado(ResultadoCarrera resultado)
        {
            try
            {
                var inscripcion = _inscripcionesService.ListarInscripciones().Find(i => i.Codigo == resultado.CodigoInscripcion);
                if (inscripcion != null)
                {
                    var evento = _eventosService.ListarEventos().Find(e => e.Codigo == inscripcion.CodigoEvento);
                    if (evento != null)
                    {
                        double porcentaje = resultado.Posicion switch
                        {
                            1 => 0.60,
                            2 => 0.25,
                            3 => 0.15,
                            _ => 0.0
                        };
                        resultado.PremioObtenido = evento.PremioTotal * porcentaje;
                    }
                }
                
                _resultadosService.InsertarResultado(resultado);
                TempData["Exito"] = "Resultado registrado con éxito.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al registrar resultado: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        [HttpPost][ValidateAntiForgeryToken]
        [Authorize(Roles = "2")]
        public IActionResult EliminarResultado(string codigo)
        {
            try
            {
                _resultadosService.EliminarResultado(codigo);
                TempData["Exito"] = "Resultado eliminado.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
    }
}

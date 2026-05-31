using System.Collections.Generic;
using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class InventarioService
    {
        private readonly InventarioRepository _inventarioRepository;

        public InventarioService(InventarioRepository inventarioRepository)
        {
            _inventarioRepository = inventarioRepository;
        }

        // ─── SUMINISTROS ─────────────────────────────────────────────────────────

        public List<Suministro> ListarSuministros() =>
            _inventarioRepository.ListarSuministros();

        public void InsertarSuministro(Suministro suministro) =>
            _inventarioRepository.InsertarSuministro(suministro);

        public void ActualizarSuministro(Suministro suministro) =>
            _inventarioRepository.ActualizarSuministro(suministro);

        public void EliminarSuministro(string codigo) =>
            _inventarioRepository.EliminarSuministro(codigo);

        // ─── ALIMENTACIÓN ────────────────────────────────────────────────────────

        public List<Alimentacion> ListarAlimentacion() =>
            _inventarioRepository.ListarAlimentacion();

        public void InsertarAlimentacion(Alimentacion alimentacion) =>
            _inventarioRepository.InsertarAlimentacion(alimentacion);

        public void ActualizarAlimentacion(Alimentacion alimentacion) =>
            _inventarioRepository.ActualizarAlimentacion(alimentacion);

        public void EliminarAlimentacion(string codigo) =>
            _inventarioRepository.EliminarAlimentacion(codigo);
    }
}

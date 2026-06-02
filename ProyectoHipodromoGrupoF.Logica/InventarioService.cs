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

        public void InsertarSuministro(Suministro suministro, string usuarioActual) =>
    _inventarioRepository.InsertarSuministro(suministro, usuarioActual);

        public void ActualizarSuministro(Suministro suministro, string usuarioActual) =>
            _inventarioRepository.ActualizarSuministro(suministro, usuarioActual);

        public void EliminarSuministro(string codigo, string usuarioActual) =>
            _inventarioRepository.EliminarSuministro(codigo, usuarioActual);

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

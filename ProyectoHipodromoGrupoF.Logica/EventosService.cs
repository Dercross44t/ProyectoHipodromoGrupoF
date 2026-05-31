using System.Collections.Generic;
using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class EventosService
    {
        private readonly EventosRepository _eventosRepository;

        public EventosService(EventosRepository eventosRepository)
        {
            _eventosRepository = eventosRepository;
        }

        // ─── EVENTOS ─────────────────────────────────────────────────────────────

        public List<Evento> ListarEventos() => _eventosRepository.ListarEventos();
        public void InsertarEvento(Evento evento, string usuarioActual) => _eventosRepository.InsertarEvento(evento, usuarioActual);
        public void ActualizarEvento(Evento evento, string usuarioActual) => _eventosRepository.ActualizarEvento(evento, usuarioActual);
        public void EliminarEvento(string codigo, string usuarioActual) => _eventosRepository.EliminarEvento(codigo, usuarioActual);

        // ─── CARRERAS ─────────────────────────────────────────────────────────────

        public List<Carrera> ListarCarreras() => _eventosRepository.ListarCarreras();
        public void InsertarCarrera(Carrera carrera) => _eventosRepository.InsertarCarrera(carrera);
        public void ActualizarCarrera(Carrera carrera) => _eventosRepository.ActualizarCarrera(carrera);
        public void EliminarCarrera(string codigo) => _eventosRepository.EliminarCarrera(codigo);
    }
}

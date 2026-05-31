using System;
using System.Collections.Generic;
using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class InscripcionesService
    {
        private readonly InscripcionesRepository _inscripcionesRepository;

        public InscripcionesService(InscripcionesRepository inscripcionesRepository)
        {
            _inscripcionesRepository = inscripcionesRepository;
        }

        public List<Inscripcion> ListarInscripciones() =>
            _inscripcionesRepository.ListarInscripciones();

        public List<Inscripcion> ListarInscripcionesPorPropietario(string codigoPropietario) =>
            _inscripcionesRepository.ListarInscripcionesPorPropietario(codigoPropietario);

        /// <summary>
        /// Valida la certificación veterinaria antes de inscribir.
        /// Si el caballo no tiene certificación vigente, lanza una excepción.
        /// </summary>
        public void InsertarInscripcion(Inscripcion inscripcion)
        {
            if (!_inscripcionesRepository.TieneCertificacionVigente(inscripcion.CodigoCaballo))
                throw new InvalidOperationException(
                    "El caballo no posee una certificación veterinaria vigente. " +
                    "Debe registrar un control veterinario antes de inscribirlo en una carrera.");

            inscripcion.IdCatEstadoInscripcion = 1; // Estado: Pendiente de validación
            _inscripcionesRepository.InsertarInscripcion(inscripcion);
        }

        public void ActualizarInscripcion(Inscripcion inscripcion) =>
            _inscripcionesRepository.ActualizarInscripcion(inscripcion);

        public void EliminarInscripcion(string codigo) =>
            _inscripcionesRepository.EliminarInscripcion(codigo);

        public bool TieneCertificacionVigente(string codigoCaballo) =>
            _inscripcionesRepository.TieneCertificacionVigente(codigoCaballo);
    }
}

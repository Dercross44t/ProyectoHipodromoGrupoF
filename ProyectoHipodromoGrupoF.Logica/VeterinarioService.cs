using System.Collections.Generic;
using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class VeterinarioService
    {
        private readonly VeterinarioRepository _veterinarioRepository;

        public VeterinarioService(VeterinarioRepository veterinarioRepository)
        {
            _veterinarioRepository = veterinarioRepository;
        }

        // ─── VETERINARIOS ────────────────────────────────────────────────────────

        public List<Veterinario> ListarVeterinarios() =>
            _veterinarioRepository.ListarVeterinarios();

        public void InsertarVeterinario(Veterinario veterinario) =>
            _veterinarioRepository.InsertarVeterinario(veterinario);

        public void ActualizarVeterinario(Veterinario veterinario) =>
            _veterinarioRepository.ActualizarVeterinario(veterinario);

        public void EliminarVeterinario(string codigo) =>
            _veterinarioRepository.EliminarVeterinario(codigo);

        // ─── HISTORIAL VETERINARIO ───────────────────────────────────────────────

        public List<HistorialVeterinario> ListarHistorialVeterinario() =>
            _veterinarioRepository.ListarHistorialVeterinario();

        public List<HistorialVeterinario> ListarHistorialPorCaballo(string codigoCaballo) =>
            _veterinarioRepository.ListarHistorialPorCaballo(codigoCaballo);

        public void InsertarHistorial(HistorialVeterinario historial) =>
            _veterinarioRepository.InsertarHistorial(historial);

        public void ActualizarHistorial(HistorialVeterinario historial) =>
            _veterinarioRepository.ActualizarHistorial(historial);

        public void EliminarHistorial(string codigo) =>
            _veterinarioRepository.EliminarHistorial(codigo);

        // ─── ALERTAS (2% rúbrica) ────────────────────────────────────────────────

        /// <summary>
        /// Retorna caballos con certificación vencida o que vence en los próximos 30 días.
        /// </summary>
        public List<AlertaVeterinaria> ObtenerAlertasCertificacion() =>
            _veterinarioRepository.ObtenerAlertasCertificacion();
    }
}

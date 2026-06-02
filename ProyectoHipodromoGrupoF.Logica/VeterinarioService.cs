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

        public void InsertarVeterinario(Veterinario veterinario, string usuarioActual) =>
            _veterinarioRepository.InsertarVeterinario(veterinario, usuarioActual);

        public void InsertarVeterinario(Veterinario veterinario) =>
            _veterinarioRepository.InsertarVeterinario(veterinario, "sistema");

        public void ActualizarVeterinario(Veterinario veterinario, string usuarioActual) =>
            _veterinarioRepository.ActualizarVeterinario(veterinario, usuarioActual);

        public void EliminarVeterinario(string codigo, string usuarioActual) =>
            _veterinarioRepository.EliminarVeterinario(codigo, usuarioActual);

        public void ActualizarVeterinario(Veterinario veterinario) =>
            _veterinarioRepository.ActualizarVeterinario(veterinario, "sistema");

        public void EliminarVeterinario(string codigo) =>
            _veterinarioRepository.EliminarVeterinario(codigo, "sistema");

        public string ObtenerCodigoVeterinarioPorUsuario(string usuarioActual) =>
            _veterinarioRepository.ObtenerCodigoVeterinarioPorUsuario(usuarioActual);

        // ─── HISTORIAL VETERINARIO ───────────────────────────────────────────────

        public List<HistorialVeterinario> ListarHistorialVeterinario() =>
            _veterinarioRepository.ListarHistorialVeterinario();


        public List<HistorialVeterinario> ListarHistorialPorCaballo(string codigoCaballo) =>
            _veterinarioRepository.ListarHistorialPorCaballo(codigoCaballo);

        public void InsertarHistorial(HistorialVeterinario historial, string usuarioActual) =>
            _veterinarioRepository.InsertarHistorial(historial, usuarioActual);

        public void InsertarHistorial(HistorialVeterinario historial) =>
            _veterinarioRepository.InsertarHistorial(historial, "sistema");

        public void ActualizarHistorial(HistorialVeterinario historial, string usuarioActual) =>
            _veterinarioRepository.ActualizarHistorial(historial, usuarioActual);

        public void EliminarHistorial(string codigo, string usuarioActual) =>
            _veterinarioRepository.EliminarHistorial(codigo, usuarioActual);

        public void ActualizarHistorial(HistorialVeterinario historial) =>
            _veterinarioRepository.ActualizarHistorial(historial, "sistema");

        public void EliminarHistorial(string codigo) =>
            _veterinarioRepository.EliminarHistorial(codigo, "sistema");

        // ─── ALERTAS ─────────────────────────────────────────────────────────────

        public List<AlertaVeterinaria> ObtenerAlertasCertificacion() =>
            _veterinarioRepository.ObtenerAlertasCertificacion();
    }
}

using System.Collections.Generic;
using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public Usuario? ValidarUsuario(string nombreUsuario, string contrasena) =>
            _usuarioRepository.ValidarUsuario(nombreUsuario, contrasena);

        public List<Usuario> ListarUsuarios() =>
            _usuarioRepository.ListarUsuarios();

        public void InsertarUsuario(Usuario usuario, string usuarioActual) =>
            _usuarioRepository.InsertarUsuario(usuario, usuarioActual);

        public void InsertarUsuario(Usuario usuario) =>
            _usuarioRepository.InsertarUsuario(usuario, "sistema");

        public void ActualizarUsuario(Usuario usuario, string usuarioActual) =>
            _usuarioRepository.ActualizarUsuario(usuario, usuarioActual);

        public void EliminarUsuario(string codigo, string usuarioActual) =>
            _usuarioRepository.EliminarUsuario(codigo, usuarioActual);

        public void ActualizarUsuario(Usuario usuario) =>
            _usuarioRepository.ActualizarUsuario(usuario, "sistema");

        public void EliminarUsuario(string codigo) =>
            _usuarioRepository.EliminarUsuario(codigo, "sistema");

        public string? ObtenerCodigoPropietarioPorCedula(string cedula) =>
            _usuarioRepository.ObtenerCodigoPropietarioPorCedula(cedula);
    }
}

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

        public void InsertarUsuario(Usuario usuario) =>
            _usuarioRepository.InsertarUsuario(usuario);

        public void ActualizarUsuario(Usuario usuario) =>
            _usuarioRepository.ActualizarUsuario(usuario);

        public void EliminarUsuario(string codigo) =>
            _usuarioRepository.EliminarUsuario(codigo);

        /// <summary>
        /// Dado la cédula de un propietario, devuelve su código de propietario (ej. "pro0001").
        /// Usado al construir los claims de sesión en el AuthController.
        /// </summary>
        public string? ObtenerCodigoPropietarioPorCedula(string cedula) =>
            _usuarioRepository.ObtenerCodigoPropietarioPorCedula(cedula);
    }
}

using System;
using System.Collections.Generic;
using Npgsql;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class UsuarioRepository
    {
        private readonly ConexionDB _conexionDB;

        public UsuarioRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        // ─── AUTENTICACIÓN ────────────────────────────────────────────────────────

        /// <summary>Valida usuario comparando contraseña en texto plano.</summary>
        public Usuario? ValidarUsuario(string nombreUsuario, string contrasena)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_usuario()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                var usernameDB = lector.GetString(2);
                var passDB     = lector.GetString(3);

                if (usernameDB == nombreUsuario && passDB == contrasena)
                {
                    return new Usuario
                    {
                        Codigo     = lector.GetString(0),
                        Cedula     = lector.GetString(1),
                        Nombre     = usernameDB,
                        Contrasena = string.Empty, // Nunca retornar hash al cliente
                        IdCatRol   = lector.GetInt32(4)
                    };
                }
            }
            return null;
        }

        // ─── CRUD ─────────────────────────────────────────────────────────────────

        public List<Usuario> ListarUsuarios()
        {
            var usuarios = new List<Usuario>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_usuario()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                usuarios.Add(new Usuario
                {
                    Codigo     = lector.GetString(0),
                    Cedula     = lector.GetString(1),
                    Nombre     = lector.GetString(2),
                    Contrasena = string.Empty,
                    IdCatRol   = lector.GetInt32(4)
                });
            }
            return usuarios;
        }

        public void InsertarUsuario(Usuario usuario)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "CALL public.insertar_usuario($1,$2,$3,$4)", conexion);
            comando.Parameters.AddWithValue(usuario.Cedula);
            comando.Parameters.AddWithValue(usuario.Nombre);
            comando.Parameters.AddWithValue(usuario.Contrasena);
            comando.Parameters.AddWithValue(usuario.IdCatRol);
            comando.ExecuteNonQuery();
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            // Si llega contraseña vacía, mantener la contraseña actual de BD
            var contrasena = string.IsNullOrWhiteSpace(usuario.Contrasena)
                             ? ObtenerHashActual(usuario.Codigo)
                             : usuario.Contrasena;

            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_usuario($1,$2,$3,$4,$5)", conexion);
            comando.Parameters.AddWithValue(usuario.Codigo);
            comando.Parameters.AddWithValue(usuario.Cedula);
            comando.Parameters.AddWithValue(usuario.Nombre);
            comando.Parameters.AddWithValue(contrasena);
            comando.Parameters.AddWithValue(usuario.IdCatRol);
            comando.ExecuteNonQuery();
        }

        public void EliminarUsuario(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.eliminar_usuario($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        private string ObtenerHashActual(string codigoUsuario)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand(
                "SELECT * FROM public.obtener_contrasena_usuario($1)", conexion);
            cmd.Parameters.AddWithValue(codigoUsuario);
            var result = cmd.ExecuteScalar();
            return result?.ToString() ?? string.Empty;
        }

        // ─── PROPIETARIO LOOKUP ───────────────────────────────────────────────────

        /// <summary>
        /// Dado la cédula de un usuario con rol Propietario, devuelve su código de propietario
        /// (ej. "pro0001"). Retorna null si no existe registro de propietario para esa cédula.
        /// </summary>
        public string? ObtenerCodigoPropietarioPorCedula(string cedula)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand(
                "SELECT public.obtener_codigo_propietario_por_cedula($1)", conexion);
            cmd.Parameters.AddWithValue(cedula);
            var result = cmd.ExecuteScalar();
            return result == DBNull.Value || result == null ? null : result.ToString();
        }
    }
}

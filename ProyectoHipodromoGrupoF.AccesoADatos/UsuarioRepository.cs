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

        private static void EstablecerUsuarioActual(NpgsqlConnection conexion, string usuarioActual)
        {
            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual ?? "usuario_desconocido");
            comandoUsuario.ExecuteNonQuery();
        }

        // ─── AUTENTICACIÓN ───────────────────────────────────────────────────────

        public Usuario? ValidarUsuario(string nombreUsuario, string contrasena)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_usuario()", conexion);
            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                var usernameDB = lector.GetString(2);
                var passDB = lector.GetString(3);

                if (usernameDB == nombreUsuario && passDB == contrasena)
                {
                    return new Usuario
                    {
                        Codigo = lector.GetString(0),
                        Cedula = lector.GetString(1),
                        Nombre = usernameDB,
                        Contrasena = string.Empty,
                        IdCatRol = lector.GetInt32(4)
                    };
                }
            }

            return null;
        }

        // ─── USUARIOS ────────────────────────────────────────────────────────────

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
                    Codigo = lector.GetString(0),
                    Cedula = lector.GetString(1),
                    Nombre = lector.GetString(2),
                    Contrasena = string.Empty,
                    IdCatRol = lector.GetInt32(4)
                });
            }

            return usuarios;
        }

        public void InsertarUsuario(Usuario usuario, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

            using var comando = new NpgsqlCommand(
                "CALL public.insertar_usuario($1,$2,$3,$4)", conexion);

            comando.Parameters.AddWithValue(usuario.Cedula);
            comando.Parameters.AddWithValue(usuario.Nombre);
            comando.Parameters.AddWithValue(usuario.Contrasena);
            comando.Parameters.AddWithValue(usuario.IdCatRol);
            comando.ExecuteNonQuery();
        }

        public void ActualizarUsuario(Usuario usuario, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

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

        public void EliminarUsuario(string codigo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

            using var comando = new NpgsqlCommand(
                "CALL public.eliminar_usuario($1)", conexion);

            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        private string ObtenerHashActual(string codigoUsuario)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.obtener_contrasena_usuario($1)", conexion);

            comando.Parameters.AddWithValue(codigoUsuario);

            var resultado = comando.ExecuteScalar();
            return resultado?.ToString() ?? string.Empty;
        }

        public string? ObtenerCodigoPropietarioPorCedula(string cedula)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT public.obtener_codigo_propietario_por_cedula($1)", conexion);

            comando.Parameters.AddWithValue(cedula);

            var resultado = comando.ExecuteScalar();
            return resultado == DBNull.Value || resultado == null ? null : resultado.ToString();
        }
    }
}

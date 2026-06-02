using System;
using System.Collections.Generic;
using Npgsql;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class EquinosRepository
    {
        private readonly ConexionDB _conexionDB;

        public EquinosRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        // ─── CABALLOS ───────────────────────────────────────────────────────────

        public List<Caballo> ListarCaballos()
        {
            var caballos = new List<Caballo>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_caballo()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                caballos.Add(MapearCaballo(lector));
            }
            return caballos;
        }

        public void AsignarCaballoEstablo(string codigoCaballo, string codigoEstablo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual);
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.asignar_caballo_establo($1,$2)", conexion);

            comando.Parameters.AddWithValue(codigoCaballo);
            comando.Parameters.AddWithValue(codigoEstablo);

            comando.ExecuteNonQuery();
        }

        public void CambiarEstadoEstablo(string codigoEstablo, int idCatEstadoEstablo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual);
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.cambiar_estado_establo($1,$2)", conexion);

            comando.Parameters.AddWithValue(codigoEstablo);
            comando.Parameters.AddWithValue(idCatEstadoEstablo);

            comando.ExecuteNonQuery();
        }

        public void QuitarCaballoEstablo(string codigoCaballo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual);
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.quitar_caballo_establo($1)", conexion);

            comando.Parameters.AddWithValue(codigoCaballo);

            comando.ExecuteNonQuery();
        }

        public List<Caballo> ListarCaballosPorPropietario(string codigoPropietario)
        {
            var caballos = new List<Caballo>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_caballos_por_propietario($1)", conexion);
            comando.Parameters.AddWithValue(codigoPropietario);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                caballos.Add(MapearCaballo(lector));
            }
            return caballos;
        }

        public void InsertarCaballo(Caballo caballo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual);
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.insertar_caballo($1,$2,$3,$4,$5,$6,$7,$8)", conexion);

            comando.Parameters.AddWithValue(caballo.CodigoPropietario);
            comando.Parameters.AddWithValue(string.IsNullOrWhiteSpace(caballo.CodigoEstablo)
                ? DBNull.Value : caballo.CodigoEstablo);
            comando.Parameters.AddWithValue(caballo.Nombre);
            comando.Parameters.AddWithValue(caballo.FechaNacimiento);
            comando.Parameters.AddWithValue(caballo.IdCatSexo);
            comando.Parameters.AddWithValue(caballo.IdCatRaza);
            comando.Parameters.AddWithValue(caballo.Peso);
            comando.Parameters.AddWithValue(caballo.IdCatEstadoCaballo.HasValue
                ? caballo.IdCatEstadoCaballo.Value : DBNull.Value);

            comando.ExecuteNonQuery();
        }

        public void ActualizarCaballo(Caballo caballo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual);
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_caballo($1,$2,$3,$4,$5,$6,$7,$8,$9)", conexion);
            comando.Parameters.AddWithValue(caballo.Codigo);
            comando.Parameters.AddWithValue(caballo.CodigoPropietario);
            comando.Parameters.AddWithValue(
        string.IsNullOrWhiteSpace(caballo.CodigoEstablo)
        ? DBNull.Value
        : caballo.CodigoEstablo
);
            comando.Parameters.AddWithValue(caballo.Nombre);
            comando.Parameters.AddWithValue(caballo.FechaNacimiento);
            comando.Parameters.AddWithValue(caballo.IdCatSexo);
            comando.Parameters.AddWithValue(caballo.IdCatRaza);
            comando.Parameters.AddWithValue(caballo.Peso);
            comando.Parameters.AddWithValue(caballo.IdCatEstadoCaballo);
            comando.ExecuteNonQuery();
        }

        public void EliminarCaballo(string codigo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual);
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand("CALL public.eliminar_caballo($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        public void ActualizarEstadoCaballo(string codigoCaballo, int idCatEstadoCaballo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual);
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_estado_caballo($1,$2)", conexion);

            comando.Parameters.AddWithValue(codigoCaballo);
            comando.Parameters.AddWithValue(idCatEstadoCaballo);

            comando.ExecuteNonQuery();
        }

        private static Caballo MapearCaballo(NpgsqlDataReader lector) => new Caballo
        {
            Codigo             = lector.GetString(0),
            CodigoPropietario  = lector.IsDBNull(1) ? string.Empty : lector.GetString(1),
            CodigoEstablo      = lector.IsDBNull(2) ? string.Empty : lector.GetString(2),
            Nombre             = lector.GetString(3),
            FechaNacimiento    = lector.GetFieldValue<DateOnly>(4),
            IdCatSexo          = lector.GetInt32(5),
            IdCatRaza          = lector.GetInt32(6),
            Peso               = lector.GetDouble(7),
            IdCatEstadoCaballo = lector.IsDBNull(8) ? null : lector.GetInt32(8)
        };

        // ─── ESTABLOS ────────────────────────────────────────────────────────────

        public List<Establo> ListarEstablos()
        {
            var establos = new List<Establo>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_establo()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                establos.Add(MapearEstablo(lector));
            }
            return establos;
        }

        public void InsertarEstablo(Establo establo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.insertar_establo($1,$2,$3,$4,$5)", conexion);
            comando.Parameters.AddWithValue(establo.IdCatBarrio);
            comando.Parameters.AddWithValue(establo.Capacidad);
            comando.Parameters.AddWithValue(establo.IdCatEstadoEstablo);
            comando.Parameters.AddWithValue(establo.CodigoEncargadoEstablo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue(establo.Nombre);
            comando.ExecuteNonQuery();
        }

        public void ActualizarEstablo(Establo establo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.actualizar_establo($6,$1,$2,$3,$4,$5)", conexion);
            comando.Parameters.AddWithValue(establo.IdCatBarrio);
            comando.Parameters.AddWithValue(establo.Capacidad);
            comando.Parameters.AddWithValue(establo.IdCatEstadoEstablo);
            comando.Parameters.AddWithValue(establo.CodigoEncargadoEstablo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue(establo.Nombre);
            comando.Parameters.AddWithValue(establo.Codigo);
            comando.ExecuteNonQuery();
        }

        public void EliminarEstablo(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.eliminar_establo($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        private static Establo MapearEstablo(NpgsqlDataReader lector) => new Establo
        {
            Codigo               = lector.GetString(0),
            IdCatBarrio          = lector.GetInt32(1),
            Capacidad            = lector.GetInt32(2),
            IdCatEstadoEstablo   = lector.GetInt32(3),
            CodigoEncargadoEstablo = lector.IsDBNull(4) ? null : lector.GetString(4),
            Nombre               = (lector.FieldCount > 5 && !lector.IsDBNull(5)) ? lector.GetString(5) : string.Empty
        };

        public void EstablecerUsuarioActual(string usuario)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var command = new NpgsqlCommand("CALL establecer_usuario_actual(@usuario)", conexion);

            command.Parameters.AddWithValue("usuario", usuario);

            command.ExecuteNonQuery();
        }
    }
}

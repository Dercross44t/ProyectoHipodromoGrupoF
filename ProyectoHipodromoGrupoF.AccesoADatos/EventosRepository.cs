using System;
using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class EventosRepository
    {
        private readonly ConexionDB _conexionDB;

        public EventosRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        // ─── EVENTOS ─────────────────────────────────────────────────────────────

        public List<Evento> ListarEventos()
        {
            var eventos = new List<Evento>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_evento()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                eventos.Add(MapearEvento(lector));
            }
            return eventos;
        }

        public void InsertarEvento(Evento evento, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual ?? "usuario_desconocido");
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.insertar_evento($1,$2,$3,$4,$5)", conexion);
            comando.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, evento.Nombre);
            comando.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Date, evento.Fecha);
            comando.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Double, evento.PremioTotal);
            comando.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Integer, evento.IdCatEstadoEvento);
            comando.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, evento.CodigoCarrera);
            comando.ExecuteNonQuery();
        }

        public void ActualizarEvento(Evento evento, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual);
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_evento($1,$2,$3,$4,$5,$6)", conexion);
            comando.Parameters.AddWithValue(evento.Codigo);
            comando.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, evento.Nombre);
            comando.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Date, evento.Fecha);
            comando.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Double, evento.PremioTotal);
            comando.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Integer, evento.IdCatEstadoEvento);
            comando.Parameters.AddWithValue(NpgsqlTypes.NpgsqlDbType.Varchar, evento.CodigoCarrera);
            comando.ExecuteNonQuery();
        }

        public void EliminarEvento(string codigo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual);
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand("CALL public.eliminar_evento($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        private static Evento MapearEvento(NpgsqlDataReader lector) => new Evento
        {
            Codigo             = lector.GetString(0),
            Nombre             = lector.GetString(1),
            Fecha              = lector.GetDateTime(2),
            PremioTotal        = lector.GetDouble(3),
            IdCatEstadoEvento  = lector.GetInt32(4),
            CodigoCarrera      = lector.IsDBNull(5) ? string.Empty : lector.GetString(5)
        };

        // ─── CARRERAS ─────────────────────────────────────────────────────────────

        public List<Carrera> ListarCarreras()
        {
            var lista = new List<Carrera>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("SELECT * FROM public.listar_carrera()", conexion);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new Carrera
                {
                    Codigo          = r.GetString(0),
                    Nombre          = r.GetString(1),
                    IdCatTipoCarrera = r.GetInt32(2),
                    IdCatDistancia  = r.GetInt32(3)
                });
            return lista;
        }

        public void InsertarCarrera(Carrera carrera)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            // insertar_carrera(nombre, id_cat_tipo_carrera, id_cat_distancia)
            using var cmd = new NpgsqlCommand("CALL public.insertar_carrera($1,$2,$3)", conexion);
            cmd.Parameters.AddWithValue(carrera.Nombre);
            cmd.Parameters.AddWithValue(carrera.IdCatTipoCarrera);
            cmd.Parameters.AddWithValue(carrera.IdCatDistancia);
            cmd.ExecuteNonQuery();
        }

        public void ActualizarCarrera(Carrera carrera)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            // actualizar_carrera(codigo, nombre, id_cat_tipo_carrera, id_cat_distancia)
            using var cmd = new NpgsqlCommand("CALL public.actualizar_carrera($1,$2,$3,$4)", conexion);
            cmd.Parameters.AddWithValue(carrera.Codigo);
            cmd.Parameters.AddWithValue(carrera.Nombre);
            cmd.Parameters.AddWithValue(carrera.IdCatTipoCarrera);
            cmd.Parameters.AddWithValue(carrera.IdCatDistancia);
            cmd.ExecuteNonQuery();
        }

        public void EliminarCarrera(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("CALL public.eliminar_carrera($1)", conexion);
            cmd.Parameters.AddWithValue(codigo);
            cmd.ExecuteNonQuery();
        }
    }
}

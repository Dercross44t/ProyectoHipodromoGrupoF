using System;
using System.Collections.Generic;
using Npgsql;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class InscripcionesRepository
    {
        private readonly ConexionDB _conexionDB;

        public InscripcionesRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        public List<Inscripcion> ListarInscripciones()
        {
            var lista = new List<Inscripcion>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_inscripcion()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                lista.Add(MapearInscripcion(lector));
            }
            return lista;
        }

        public List<Inscripcion> ListarInscripcionesPorPropietario(string codigoPropietario)
        {
            var lista = new List<Inscripcion>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_inscripciones_por_propietario($1)", conexion);
            comando.Parameters.AddWithValue(codigoPropietario);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                lista.Add(MapearInscripcion(lector));
            }
            return lista;
        }

        public void InsertarInscripcion(Inscripcion inscripcion)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            // insertar_inscripcion(codigo_evento, codigo_caballo, fecha, id_cat_estado_inscripcion)
            using var comando = new NpgsqlCommand(
                "CALL public.insertar_inscripcion($1,$2,$3,$4)", conexion);
            comando.Parameters.AddWithValue(inscripcion.CodigoEvento);
            comando.Parameters.AddWithValue(inscripcion.CodigoCaballo);
            comando.Parameters.AddWithValue(DateOnly.FromDateTime(inscripcion.Fecha));
            comando.Parameters.AddWithValue(inscripcion.IdCatEstadoInscripcion);
            comando.ExecuteNonQuery();
        }
        

        public void ActualizarInscripcion(Inscripcion inscripcion)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            // actualizar_inscripcion(codigo, codigo_evento, codigo_caballo, fecha, id_cat_estado_inscripcion)
            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_inscripcion($1,$2,$3,$4,$5)", conexion);
            comando.Parameters.AddWithValue(inscripcion.Codigo);
            comando.Parameters.AddWithValue(inscripcion.CodigoEvento);
            comando.Parameters.AddWithValue(inscripcion.CodigoCaballo);
            comando.Parameters.AddWithValue(DateOnly.FromDateTime(inscripcion.Fecha));
            comando.Parameters.AddWithValue(inscripcion.IdCatEstadoInscripcion);
            comando.ExecuteNonQuery();
        }

        public void EliminarInscripcion(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.eliminar_inscripcion($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        /// <summary>
        /// Verifica si un caballo tiene certificación veterinaria vigente (no vencida a hoy).
        /// </summary>
        public bool TieneCertificacionVigente(string codigoCaballo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                @"SELECT COUNT(*) FROM public.historial_veterinario
                  WHERE codigo_caballo = $1
                    AND fecha_vencimiento_certificacion >= CURRENT_DATE", conexion);
            comando.Parameters.AddWithValue(codigoCaballo);
            var resultado = comando.ExecuteScalar();
            return Convert.ToInt64(resultado) > 0;
        }

        public List<CaballoInscripcion> ListarCaballosParaInscripcion()
        {
            var lista = new List<CaballoInscripcion>();

            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_caballos_para_inscripcion()", conexion);

            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                lista.Add(new CaballoInscripcion
                {
                    Codigo = lector.GetString(0),
                    Nombre = lector.GetString(1),
                    PuedeInscribirse = lector.GetBoolean(2),
                    Motivo = lector.IsDBNull(3) ? string.Empty : lector.GetString(3)
                });
            }

            return lista;
        }

        private static Inscripcion MapearInscripcion(NpgsqlDataReader lector) => new Inscripcion
        {
            Codigo                   = lector.GetString(0),
            CodigoEvento             = lector.GetString(1),
            CodigoCaballo            = lector.GetString(2),
            Fecha                    = lector.GetDateTime(3),
            IdCatEstadoInscripcion   = lector.GetInt32(4)
        };
    }
}

using System;
using System.Collections.Generic;
using Npgsql;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class VeterinarioRepository
    {
        private readonly ConexionDB _conexionDB;

        public VeterinarioRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        // ─── VETERINARIOS ────────────────────────────────────────────────────────

        public List<Veterinario> ListarVeterinarios()
        {
            var lista = new List<Veterinario>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_veterinario()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                lista.Add(new Veterinario
                {
                    Codigo          = lector.GetString(0),
                    NumeroColegiado = lector.GetString(1),
                    Cedula          = lector.GetString(2),
                    Especialidad    = lector.IsDBNull(3) ? string.Empty : lector.GetString(3)
                });
            }
            return lista;
        }

        public void InsertarVeterinario(Veterinario veterinario)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            // insertar_veterinario(cedula, numero_colegiado, especialidad)
            using var comando = new NpgsqlCommand(
                "CALL public.insertar_veterinario($1,$2,$3)", conexion);
            comando.Parameters.AddWithValue(veterinario.Cedula);
            comando.Parameters.AddWithValue(veterinario.NumeroColegiado);
            comando.Parameters.AddWithValue(veterinario.Especialidad ?? string.Empty);
            comando.ExecuteNonQuery();
        }

        public void ActualizarVeterinario(Veterinario veterinario)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            // actualizar_veterinario(codigo, cedula, numero_colegiado, especialidad)
            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_veterinario($1,$2,$3,$4)", conexion);
            comando.Parameters.AddWithValue(veterinario.Codigo);
            comando.Parameters.AddWithValue(veterinario.Cedula);
            comando.Parameters.AddWithValue(veterinario.NumeroColegiado);
            comando.Parameters.AddWithValue(veterinario.Especialidad ?? string.Empty);
            comando.ExecuteNonQuery();
        }

        public void EliminarVeterinario(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.eliminar_veterinario($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        // ─── HISTORIAL VETERINARIO ───────────────────────────────────────────────

        public List<HistorialVeterinario> ListarHistorialVeterinario()
        {
            var lista = new List<HistorialVeterinario>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_historial_veterinario()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                lista.Add(MapearHistorial(lector));
            }
            return lista;
        }

        public List<HistorialVeterinario> ListarHistorialPorCaballo(string codigoCaballo)
        {
            var lista = new List<HistorialVeterinario>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                @"SELECT codigo, codigo_caballo, codigo_veterinario, diagnostico, tratamiento,
                         fecha_revision, fecha_vencimiento_certificacion
                  FROM public.historial_veterinario
                  WHERE codigo_caballo = $1
                  ORDER BY fecha_revision DESC", conexion);
            comando.Parameters.AddWithValue(codigoCaballo);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                lista.Add(MapearHistorial(lector));
            }
            return lista;
        }

        public void InsertarHistorial(HistorialVeterinario historial, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual ?? "usuario_desconocido");
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.insertar_historial_veterinario($1,$2,$3,$4,$5,$6)", conexion);
            comando.Parameters.AddWithValue(historial.CodigoCaballo);
            comando.Parameters.AddWithValue(historial.CodigoVeterinario);
            comando.Parameters.AddWithValue(historial.Diagnostico);
            comando.Parameters.AddWithValue(historial.Tratamiento);
            comando.Parameters.AddWithValue(DateOnly.FromDateTime(historial.FechaRevision));
            comando.Parameters.AddWithValue(DateOnly.FromDateTime(historial.FechaVencimientoCertificacion));
            comando.ExecuteNonQuery();
        }

        public void ActualizarHistorial(HistorialVeterinario historial, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual ?? "usuario_desconocido");
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_historial_veterinario($1,$2,$3,$4,$5,$6,$7)", conexion);
            comando.Parameters.AddWithValue(historial.Codigo);
            comando.Parameters.AddWithValue(historial.CodigoCaballo);
            comando.Parameters.AddWithValue(historial.CodigoVeterinario);
            comando.Parameters.AddWithValue(historial.Diagnostico);
            comando.Parameters.AddWithValue(historial.Tratamiento);
            comando.Parameters.AddWithValue(DateOnly.FromDateTime(historial.FechaRevision));
            comando.Parameters.AddWithValue(DateOnly.FromDateTime(historial.FechaVencimientoCertificacion));
            comando.ExecuteNonQuery();
        }

        public void EliminarHistorial(string codigo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual ?? "usuario_desconocido");
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.eliminar_historial_veterinario($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }
        public List<AlertaVeterinaria> ObtenerAlertasCertificacion()
        {
            var alertas = new List<AlertaVeterinaria>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_alertas_certificacion_veterinaria()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                alertas.Add(new AlertaVeterinaria
                {
                    CodigoCaballo                = lector.GetString(0),
                    NombreCaballo                = lector.GetString(1),
                    CodigoVeterinario            = lector.GetString(2),
                    FechaRevision                = lector.GetDateTime(3),
                    FechaVencimientoCertificacion = lector.GetDateTime(4),
                    DiasRestantes                = lector.GetInt32(5)
                });
            }
            return alertas;
        }

        private static HistorialVeterinario MapearHistorial(NpgsqlDataReader lector) => new HistorialVeterinario
        {
            Codigo                        = lector.GetString(0),
            CodigoCaballo                 = lector.GetString(1),
            CodigoVeterinario             = lector.GetString(2),
            Diagnostico                   = lector.IsDBNull(3) ? string.Empty : lector.GetString(3),
            Tratamiento                   = lector.IsDBNull(4) ? string.Empty : lector.GetString(4),
            FechaRevision                 = lector.GetDateTime(5),
            FechaVencimientoCertificacion = lector.GetDateTime(6)
        };
    }
}

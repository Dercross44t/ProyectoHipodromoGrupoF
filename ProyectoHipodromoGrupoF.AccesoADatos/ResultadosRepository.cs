using System.Collections.Generic;
using Npgsql;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class ResultadosRepository
    {
        private readonly ConexionDB _conexionDB;

        public ResultadosRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        public List<ResultadoCarrera> ListarResultados()
        {
            var resultados = new List<ResultadoCarrera>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_resultado_carrera()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                resultados.Add(new ResultadoCarrera
                {
                    Codigo = lector.GetString(0),
                    CodigoInscripcion = lector.GetString(1),
                    Posicion = lector.GetInt32(2),
                    Tiempo = lector.IsDBNull(3) ? null : lector.GetString(3),
                    PremioObtenido = lector.IsDBNull(4) ? 0 : lector.GetDouble(4)
                });
            }
            return resultados;
        }

        public void InsertarResultado(ResultadoCarrera resultado)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "INSERT INTO public.resultado_carrera (codigo_inscripcion, posicion, tiempo, premio_obtenido) VALUES ($1, $2, $3, $4)", conexion);
            comando.Parameters.AddWithValue(resultado.CodigoInscripcion);
            comando.Parameters.AddWithValue(resultado.Posicion);
            comando.Parameters.AddWithValue(resultado.Tiempo ?? (object)System.DBNull.Value);
            comando.Parameters.AddWithValue(resultado.PremioObtenido);
            comando.ExecuteNonQuery();
        }

        public void ActualizarResultado(ResultadoCarrera resultado)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "UPDATE public.resultado_carrera SET codigo_inscripcion = $1, posicion = $2, tiempo = $3, premio_obtenido = $4 WHERE codigo = $5", conexion);
            comando.Parameters.AddWithValue(resultado.CodigoInscripcion);
            comando.Parameters.AddWithValue(resultado.Posicion);
            comando.Parameters.AddWithValue(resultado.Tiempo ?? (object)System.DBNull.Value);
            comando.Parameters.AddWithValue(resultado.PremioObtenido);
            comando.Parameters.AddWithValue(resultado.Codigo!);
            comando.ExecuteNonQuery();
        }

        public void EliminarResultado(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.eliminar_resultado_carrera($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }
    }
}

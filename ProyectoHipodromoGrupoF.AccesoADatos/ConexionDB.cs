using System;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class ConexionDB
    {
        private readonly string _connectionString;

        public ConexionDB(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HipodromoDB") 
                ?? throw new ArgumentNullException("La cadena de conexión 'HipodromoDB' no se encontró.");
        }

        public NpgsqlConnection ObtenerConexion()
        {
            var conexion = new NpgsqlConnection(_connectionString);
            conexion.Open();
            return conexion;
        }
    }
}

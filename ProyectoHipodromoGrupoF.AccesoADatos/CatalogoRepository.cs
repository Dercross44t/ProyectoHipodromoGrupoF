using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class CatalogoRepository
    {
        private readonly ConexionDB _conexionDB;

        public CatalogoRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        public Dictionary<string, DataTable> ListarBitacorasTrimestrales()
        {
            var resultado = new Dictionary<string, DataTable>();

            var tablas = new Dictionary<string, string>
            {
                { "Bitácora de Caballos", "public.bitacora_caballo" },
                { "Bitácora de Eventos", "public.bitacora_evento" },
                { "Bitácora de Facturas", "public.bitacora_factura" },
                { "Bitácora de Historial Veterinario", "public.bitacora_historial_veterinario" },
                { "Bitácora de Personas", "public.bitacora_persona" },
                { "Bitácora de Suministros", "public.bitacora_suministro" }
            };

            using var conexion = _conexionDB.ObtenerConexion();

            foreach (var tabla in tablas)
            {
                using var comando = new NpgsqlCommand($"SELECT * FROM {tabla.Value} ORDER BY 1 DESC", conexion);
                using var reader = comando.ExecuteReader();

                var dataTable = new DataTable();
                dataTable.Load(reader);

                resultado.Add(tabla.Key, dataTable);
            }

            return resultado;
        }
    }
}

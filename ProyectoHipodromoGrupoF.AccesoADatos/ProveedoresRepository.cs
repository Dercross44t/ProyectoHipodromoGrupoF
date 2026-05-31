using Npgsql;
using ProyectoHipodromoGrupoF.Modelo;
using System.Collections.Generic;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class ProveedoresRepository
    {
        private readonly ConexionDB _conexionDB;

        public ProveedoresRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        public List<Proveedor> ListarProveedores()
        {
            var proveedores = new List<Proveedor>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_proveedor()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                proveedores.Add(new Proveedor
                {
                    Codigo = lector.GetString(0),
                    Nombre = lector.GetString(1)
                });
            }
            return proveedores;
        }

        public void InsertarProveedor(Proveedor proveedor)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.insertar_proveedor($1)", conexion);
            comando.Parameters.AddWithValue(proveedor.Nombre);
            comando.ExecuteNonQuery();
        }

        public void ActualizarProveedor(Proveedor proveedor)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.actualizar_proveedor($1, $2)", conexion);
            comando.Parameters.AddWithValue(proveedor.Codigo!);
            comando.Parameters.AddWithValue(proveedor.Nombre);
            comando.ExecuteNonQuery();
        }

        public void EliminarProveedor(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.eliminar_proveedor($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }
    }
}

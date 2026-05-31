using System;
using System.Collections.Generic;
using Npgsql;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class InventarioRepository
    {
        private readonly ConexionDB _conexionDB;

        public InventarioRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        // ─── SUMINISTROS ─────────────────────────────────────────────────────────

        public List<Suministro> ListarSuministros()
        {
            var suministros = new List<Suministro>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_suministro()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                suministros.Add(MapearSuministro(lector));
            }
            return suministros;
        }

        public void InsertarSuministro(Suministro suministro)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            // insertar_suministro(nombre, codigo_proveedor, cantidad_disponible, fecha_ingreso, id_cat_tipo_suministro)
            using var comando = new NpgsqlCommand(
                "CALL public.insertar_suministro($1,$2,$3,$4,$5)", conexion);
            comando.Parameters.AddWithValue(suministro.Nombre);
            comando.Parameters.AddWithValue(
                string.IsNullOrEmpty(suministro.CodigoProveedor) ? DBNull.Value : (object)suministro.CodigoProveedor);
            comando.Parameters.AddWithValue(suministro.CantidadDisponible);
            comando.Parameters.AddWithValue(DateOnly.FromDateTime(suministro.FechaIngreso));
            comando.Parameters.AddWithValue(suministro.IdCatTipoSuministro);
            comando.ExecuteNonQuery();
        }

        public void ActualizarSuministro(Suministro suministro)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            // actualizar_suministro(codigo, nombre, codigo_proveedor, cantidad_disponible, fecha_ingreso, id_cat_tipo)
            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_suministro($1,$2,$3,$4,$5,$6)", conexion);
            comando.Parameters.AddWithValue(suministro.Codigo);
            comando.Parameters.AddWithValue(suministro.Nombre);
            comando.Parameters.AddWithValue(
                string.IsNullOrEmpty(suministro.CodigoProveedor) ? DBNull.Value : (object)suministro.CodigoProveedor);
            comando.Parameters.AddWithValue(suministro.CantidadDisponible);
            comando.Parameters.AddWithValue(DateOnly.FromDateTime(suministro.FechaIngreso));
            comando.Parameters.AddWithValue(suministro.IdCatTipoSuministro);
            comando.ExecuteNonQuery();
        }

        public void EliminarSuministro(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.eliminar_suministro($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        private static Suministro MapearSuministro(NpgsqlDataReader lector) => new Suministro
        {
            Codigo               = lector.GetString(0),
            Nombre               = lector.GetString(1),
            CodigoProveedor      = lector.IsDBNull(2) ? string.Empty : lector.GetString(2),
            CantidadDisponible   = lector.GetInt32(3),
            FechaIngreso         = lector.GetDateTime(4),
            IdCatTipoSuministro  = lector.GetInt32(5)
        };

        // ─── ALIMENTACIÓN ────────────────────────────────────────────────────────

        public List<Alimentacion> ListarAlimentacion()
        {
            var lista = new List<Alimentacion>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_alimentacion()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                lista.Add(MapearAlimentacion(lector));
            }
            return lista;
        }

        public void InsertarAlimentacion(Alimentacion alimentacion)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            // insertar_alimentacion(codigo_caballo, codigo_suministro, fecha, cantidad)
            using var comando = new NpgsqlCommand(
                "CALL public.insertar_alimentacion($1,$2,$3,$4)", conexion);
            comando.Parameters.AddWithValue(alimentacion.CodigoCaballo);
            comando.Parameters.AddWithValue(alimentacion.CodigoSuministro);
            comando.Parameters.AddWithValue(DateOnly.FromDateTime(alimentacion.Fecha));
            comando.Parameters.AddWithValue(alimentacion.Cantidad);
            comando.ExecuteNonQuery();
        }

        public void ActualizarAlimentacion(Alimentacion alimentacion)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            // actualizar_alimentacion(codigo, codigo_caballo, codigo_suministro, fecha, cantidad)
            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_alimentacion($1,$2,$3,$4,$5)", conexion);
            comando.Parameters.AddWithValue(alimentacion.Codigo);
            comando.Parameters.AddWithValue(alimentacion.CodigoCaballo);
            comando.Parameters.AddWithValue(alimentacion.CodigoSuministro);
            comando.Parameters.AddWithValue(DateOnly.FromDateTime(alimentacion.Fecha));
            comando.Parameters.AddWithValue(alimentacion.Cantidad);
            comando.ExecuteNonQuery();
        }

        public void EliminarAlimentacion(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.eliminar_alimentacion($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        private static Alimentacion MapearAlimentacion(NpgsqlDataReader lector) => new Alimentacion
        {
            Codigo           = lector.GetString(0),
            CodigoCaballo    = lector.GetString(1),
            CodigoSuministro = lector.GetString(2),
            Fecha            = lector.GetDateTime(3),
            Cantidad         = lector.GetInt32(4)
        };
    }
}

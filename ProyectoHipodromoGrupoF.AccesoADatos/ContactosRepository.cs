using System.Collections.Generic;
using Npgsql;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class ContactosRepository
    {
        private readonly ConexionDB _conexionDB;

        public ContactosRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        public List<TelefonoPersona> ListarTelefonosPersonas()
        {
            var lista = new List<TelefonoPersona>();

            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("SELECT codigo, cedula, numero, id_cat_tipo_telefono FROM public.telefono_persona ORDER BY codigo", conexion);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new TelefonoPersona
                {
                    Codigo = reader.GetString(0),
                    Cedula = reader.GetString(1),
                    Numero = reader.GetString(2),
                    IdCatTipoTelefono = reader.GetInt32(3)
                });
            }

            return lista;
        }

        public List<CorreoElectronicoPersona> ListarCorreosPersonas()
        {
            var lista = new List<CorreoElectronicoPersona>();

            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("SELECT codigo, cedula, correo, id_cat_tipo_correo FROM public.correo_electronico_persona ORDER BY codigo", conexion);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new CorreoElectronicoPersona
                {
                    Codigo = reader.GetString(0),
                    Cedula = reader.GetString(1),
                    Correo = reader.GetString(2),
                    IdCatTipoCorreo = reader.GetInt32(3)
                });
            }

            return lista;
        }

        public List<TelefonoProveedor> ListarTelefonosProveedores()
        {
            var lista = new List<TelefonoProveedor>();

            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("SELECT codigo, codigo_proveedor, numero, id_cat_tipo_telefono FROM public.telefono_proveedor ORDER BY codigo", conexion);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new TelefonoProveedor
                {
                    Codigo = reader.GetString(0),
                    CodigoProveedor = reader.GetString(1),
                    Numero = reader.GetString(2),
                    IdCatTipoTelefono = reader.GetInt32(3)
                });
            }

            return lista;
        }

        public List<CorreoElectronicoProveedor> ListarCorreosProveedores()
        {
            var lista = new List<CorreoElectronicoProveedor>();

            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("SELECT codigo, codigo_proveedor, correo, id_cat_tipo_correo FROM public.correo_electronico_proveedor ORDER BY codigo", conexion);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new CorreoElectronicoProveedor
                {
                    Codigo = reader.GetString(0),
                    CodigoProveedor = reader.GetString(1),
                    Correo = reader.GetString(2),
                    IdCatTipoCorreo = reader.GetInt32(3)
                });
            }

            return lista;
        }

        public void InsertarTelefonoPersona(TelefonoPersona telefono)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var cmd = new NpgsqlCommand(@"
                INSERT INTO public.telefono_persona
                (codigo, cedula, numero, id_cat_tipo_telefono)
                VALUES
                (public.generar_telefono_persona_codigo(), $1, $2, $3)", conexion);

            cmd.Parameters.AddWithValue(telefono.Cedula);
            cmd.Parameters.AddWithValue(telefono.Numero);
            cmd.Parameters.AddWithValue(telefono.IdCatTipoTelefono);

            cmd.ExecuteNonQuery();
        }

        public void InsertarCorreoPersona(CorreoElectronicoPersona correo)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var cmd = new NpgsqlCommand(@"
                INSERT INTO public.correo_electronico_persona
                (codigo, cedula, correo, id_cat_tipo_correo)
                VALUES
                (public.generar_correo_persona_codigo(), $1, $2, $3)", conexion);

            cmd.Parameters.AddWithValue(correo.Cedula);
            cmd.Parameters.AddWithValue(correo.Correo);
            cmd.Parameters.AddWithValue(correo.IdCatTipoCorreo);

            cmd.ExecuteNonQuery();
        }

        public void InsertarTelefonoProveedor(TelefonoProveedor telefono)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var cmd = new NpgsqlCommand(@"
                INSERT INTO public.telefono_proveedor
                (codigo, codigo_proveedor, numero, id_cat_tipo_telefono)
                VALUES
                (public.generar_telefono_proveedor_codigo(), $1, $2, $3)", conexion);

            cmd.Parameters.AddWithValue(telefono.CodigoProveedor);
            cmd.Parameters.AddWithValue(telefono.Numero);
            cmd.Parameters.AddWithValue(telefono.IdCatTipoTelefono);

            cmd.ExecuteNonQuery();
        }

        public void InsertarCorreoProveedor(CorreoElectronicoProveedor correo)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var cmd = new NpgsqlCommand(@"
                INSERT INTO public.correo_electronico_proveedor
                (codigo, codigo_proveedor, correo, id_cat_tipo_correo)
                VALUES
                (public.generar_correo_proveedor_codigo(), $1, $2, $3)", conexion);

            cmd.Parameters.AddWithValue(correo.CodigoProveedor);
            cmd.Parameters.AddWithValue(correo.Correo);
            cmd.Parameters.AddWithValue(correo.IdCatTipoCorreo);

            cmd.ExecuteNonQuery();
        }
    }
}
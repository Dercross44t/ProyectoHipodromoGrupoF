using System.Collections.Generic;
using Npgsql;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    /// <summary>
    /// Centraliza la lectura de todos los catálogos de la BD.
    /// Cada método retorna CatalogoItem (Id + Nombre) para llenar dropdowns.
    /// </summary>
    public class CatalogosRepository
    {
        private readonly ConexionDB _conexionDB;

        public CatalogosRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        // ─── HELPER ──────────────────────────────────────────────────────────────

        private List<CatalogoItem> LeerCatalogo(string sql, object? param = null)
        {
            var lista = new List<CatalogoItem>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand(sql, conexion);
            if (param != null) cmd.Parameters.AddWithValue(param);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new CatalogoItem { Id = r.GetInt32(0), Nombre = r.GetString(1) });
            return lista;
        }

        // ─── CATÁLOGOS EQUINOS ────────────────────────────────────────────────────

        public List<CatalogoItem> ListarRazas() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_raza()");

        public List<CatalogoItem> ListarSexos() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_sexo()");

        public List<CatalogoItem> ListarEstadosCaballo() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_estado_caballo()");

        public List<CatalogoItem> ListarEstadosEstablo() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_estado_establo()");

        // ─── CATÁLOGOS EVENTOS / CARRERAS ─────────────────────────────────────────

        public List<CatalogoItem> ListarTiposCarrera() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_tipo_carrera()");

        public List<CatalogoItem> ListarDistancias() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_distancia()");

        public List<CatalogoItem> ListarEstadosEvento() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_estado_evento()");

        // ─── CATÁLOGOS INSCRIPCIONES ──────────────────────────────────────────────

        public List<CatalogoItem> ListarEstadosInscripcion() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_estado_inscripcion()");

        // ─── CATÁLOGOS FACTURACIÓN ────────────────────────────────────────────────

        public List<CatalogoItem> ListarEstadosPago() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_estado_pago()");

        public List<CatalogoItem> ListarMetodosPago() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_metodo_pago()");

        // ─── CATÁLOGOS INVENTARIO ─────────────────────────────────────────────────

        public List<CatalogoItem> ListarTiposSuministro() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_tipo_suministro()");

        // ─── CATÁLOGOS USUARIOS ───────────────────────────────────────────────────

        public List<CatalogoItem> ListarRoles() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_rol()");

        // ─── CATÁLOGOS CONTACTO ───────────────────────────────────────────────────

        public List<CatalogoItem> ListarTiposTelefono() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_tipo_telefono()");

        public List<CatalogoItem> ListarTiposCorreo() =>
            LeerCatalogo("SELECT * FROM public.listar_cat_tipo_correo()");

        // ─── GEOGRAFÍA EN CASCADA ─────────────────────────────────────────────────

        public List<CatProvincia> ListarProvincias()
        {
            var lista = new List<CatProvincia>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand(
                "SELECT * FROM public.listar_cat_provincia()", conexion);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new CatProvincia { Id = r.GetInt32(0), Nombre = r.GetString(1) });
            return lista;
        }

        public List<CatCanton> ListarCantonesPorProvincia(int idProvincia)
        {
            var lista = new List<CatCanton>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand(
                "SELECT * FROM public.listar_cat_canton($1)",
                conexion);
            cmd.Parameters.AddWithValue(idProvincia);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new CatCanton { Id = r.GetInt32(0), IdProvincia = r.GetInt32(1), Nombre = r.GetString(2) });
            return lista;
        }

        public List<CatDistrito> ListarDistritosPorCanton(int idCanton)
        {
            var lista = new List<CatDistrito>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand(
                "SELECT * FROM public.listar_cat_distrito($1)",
                conexion);
            cmd.Parameters.AddWithValue(idCanton);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new CatDistrito { Id = r.GetInt32(0), IdCanton = r.GetInt32(1), Nombre = r.GetString(2) });
            return lista;
        }

        public List<CatBarrio> ListarBarriosPorDistrito(int idDistrito)
        {
            var lista = new List<CatBarrio>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand(
                "SELECT * FROM public.listar_cat_barrio($1)",
                conexion);
            cmd.Parameters.AddWithValue(idDistrito);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new CatBarrio { Id = r.GetInt32(0), IdDistrito = r.GetInt32(1), Nombre = r.GetString(2) });
            return lista;
        }
    }
}

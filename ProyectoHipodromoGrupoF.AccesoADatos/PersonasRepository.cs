using System;
using System.Collections.Generic;
using Npgsql;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class PersonasRepository
    {
        private readonly ConexionDB _conexionDB;

        public PersonasRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        // ─── PERSONAS ────────────────────────────────────────────────────────────

        public List<Persona> ListarPersonas()
        {
            var personas = new List<Persona>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_persona()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                personas.Add(MapearPersona(lector));
            }
            return personas;
        }

        public void InsertarPersona(Persona persona)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "CALL public.insertar_persona($1,$2,$3,$4,$5,$6)", conexion);
            comando.Parameters.AddWithValue(persona.Cedula);
            comando.Parameters.AddWithValue(persona.Nombre1 ?? string.Empty);
            comando.Parameters.AddWithValue(persona.Nombre2 ?? string.Empty);
            comando.Parameters.AddWithValue(persona.Apellido1 ?? string.Empty);
            comando.Parameters.AddWithValue(persona.Apellido2 ?? string.Empty);
            comando.Parameters.AddWithValue(persona.IdCatBarrio);
            comando.ExecuteNonQuery();
        }

        public void ActualizarPersona(Persona persona)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_persona($1,$2,$3,$4,$5,$6)", conexion);
            comando.Parameters.AddWithValue(persona.Cedula);
            comando.Parameters.AddWithValue(persona.Nombre1 ?? string.Empty);
            comando.Parameters.AddWithValue(persona.Nombre2 ?? string.Empty);
            comando.Parameters.AddWithValue(persona.Apellido1 ?? string.Empty);
            comando.Parameters.AddWithValue(persona.Apellido2 ?? string.Empty);
            comando.Parameters.AddWithValue(persona.IdCatBarrio);
            comando.ExecuteNonQuery();
        }

        public void EliminarPersona(string cedula)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.eliminar_persona($1)", conexion);
            comando.Parameters.AddWithValue(cedula);
            comando.ExecuteNonQuery();
        }

        public Persona? ObtenerPersonaPorCedula(string cedula)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand(
                "SELECT * FROM public.listar_persona() WHERE cedula = $1", conexion);
            cmd.Parameters.AddWithValue(cedula);
            using var r = cmd.ExecuteReader();
            return r.Read() ? MapearPersona(r) : null;
        }

        private static Persona MapearPersona(NpgsqlDataReader lector) => new Persona
        {
            Cedula      = lector.GetString(0),
            Nombre1     = lector.IsDBNull(1) ? string.Empty : lector.GetString(1),
            Nombre2     = lector.IsDBNull(2) ? string.Empty : lector.GetString(2),
            Apellido1   = lector.IsDBNull(3) ? string.Empty : lector.GetString(3),
            Apellido2   = lector.IsDBNull(4) ? string.Empty : lector.GetString(4),
            IdCatBarrio = lector.GetInt32(5)
        };

        // ─── TELÉFONOS DE PERSONA ─────────────────────────────────────────────────

        public void InsertarTelefonoPersona(string cedula, string numero, int idCatTipoTelefono)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("CALL public.insertar_telefono_persona($1,$2,$3)", conexion);
            cmd.Parameters.AddWithValue(cedula);
            cmd.Parameters.AddWithValue(numero);
            cmd.Parameters.AddWithValue(idCatTipoTelefono);
            cmd.ExecuteNonQuery();
        }

        public List<TelefonoPersona> ListarTelefonosPersona(string cedula)
        {
            var lista = new List<TelefonoPersona>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("SELECT * FROM public.listar_telefono_persona($1)", conexion);
            cmd.Parameters.AddWithValue(cedula);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new TelefonoPersona
                {
                    Codigo            = r.GetString(0),
                    Cedula            = r.GetString(1),
                    Numero            = r.GetString(2),
                    IdCatTipoTelefono = r.GetInt32(3)
                });
            return lista;
        }

        public void EliminarTelefonoPersona(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("CALL public.eliminar_telefono_persona($1)", conexion);
            cmd.Parameters.AddWithValue(codigo);
            cmd.ExecuteNonQuery();
        }

        // ─── CORREOS DE PERSONA ───────────────────────────────────────────────────

        public void InsertarCorreoPersona(string cedula, string correo, int idCatTipoCorreo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand(
                "CALL public.insertar_correo_electronico_persona($1,$2,$3)", conexion);
            cmd.Parameters.AddWithValue(cedula);
            cmd.Parameters.AddWithValue(correo);
            cmd.Parameters.AddWithValue(idCatTipoCorreo);
            cmd.ExecuteNonQuery();
        }

        public List<CorreoElectronicoPersona> ListarCorreosPersona(string cedula)
        {
            var lista = new List<CorreoElectronicoPersona>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand(
                "SELECT * FROM public.listar_correo_electronico_persona($1)", conexion);
            cmd.Parameters.AddWithValue(cedula);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new CorreoElectronicoPersona
                {
                    Codigo          = r.GetString(0),
                    Cedula          = r.GetString(1),
                    Correo          = r.GetString(2),
                    IdCatTipoCorreo = r.GetInt32(3)
                });
            return lista;
        }

        public void EliminarCorreoPersona(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("CALL public.eliminar_correo_electronico_persona($1)", conexion);
            cmd.Parameters.AddWithValue(codigo);
            cmd.ExecuteNonQuery();
        }

        // ─── PROPIETARIOS ────────────────────────────────────────────────────────

        public List<Propietario> ListarPropietarios()
        {
            var propietarios = new List<Propietario>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_propietario()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
                propietarios.Add(MapearPropietario(lector));
            return propietarios;
        }

        public Propietario? ObtenerPropietario(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_propietario() WHERE codigo = $1", conexion);
            comando.Parameters.AddWithValue(codigo);
            using var lector = comando.ExecuteReader();
            if (lector.Read()) return MapearPropietario(lector);
            return null;
        }

        public void InsertarPropietario(Propietario propietario)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.insertar_propietario($1,$2)", conexion);
            comando.Parameters.AddWithValue(propietario.Cedula);
            comando.Parameters.AddWithValue(propietario.DescuentoProximaFacturacion);
            comando.ExecuteNonQuery();
        }

        public void ActualizarPropietario(Propietario propietario)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.actualizar_propietario($1,$2,$3)", conexion);
            comando.Parameters.AddWithValue(propietario.Codigo);
            comando.Parameters.AddWithValue(propietario.Cedula);
            comando.Parameters.AddWithValue(propietario.DescuentoProximaFacturacion);
            comando.ExecuteNonQuery();
        }

        public void EliminarPropietario(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("CALL public.eliminar_propietario($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        public void ReiniciarDescuento(string codigoPropietario)
        {
            var prop = ObtenerPropietario(codigoPropietario);
            if (prop == null) return;
            prop.DescuentoProximaFacturacion = false;
            ActualizarPropietario(prop);
        }

        private static Propietario MapearPropietario(NpgsqlDataReader lector) => new Propietario
        {
            Codigo                      = lector.GetString(0),
            Cedula                      = lector.GetString(1),
            DescuentoProximaFacturacion = lector.GetBoolean(2)
        };

        // ─── ENCARGADOS DE ESTABLO ────────────────────────────────────────────────

        public List<EncargadoEstablo> ListarEncargados()
        {
            var lista = new List<EncargadoEstablo>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("SELECT * FROM public.listar_encargado_establo()", conexion);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                lista.Add(new EncargadoEstablo { Codigo = r.GetString(0), Cedula = r.GetString(1) });
            return lista;
        }

        public void InsertarEncargado(EncargadoEstablo encargado)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("CALL public.insertar_encargado_establo($1)", conexion);
            cmd.Parameters.AddWithValue(encargado.Cedula);
            cmd.ExecuteNonQuery();
        }

        public void EliminarEncargado(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("CALL public.eliminar_encargado_establo($1)", conexion);
            cmd.Parameters.AddWithValue(codigo);
            cmd.ExecuteNonQuery();
        }
    }
}

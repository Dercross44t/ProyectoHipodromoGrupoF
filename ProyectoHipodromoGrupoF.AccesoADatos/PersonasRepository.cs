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

        private static void EstablecerUsuarioActual(NpgsqlConnection conexion, string usuarioActual)
        {
            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual ?? "usuario_desconocido");
            comandoUsuario.ExecuteNonQuery();
        }


        // ─── PERSONAS ────────────────────────────────────────────────────────────

        public List<PersonaCombo> ListarPersonasSinRol()
        {
            var personas = new List<PersonaCombo>();

            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_personas_sin_rol()",
                conexion);

            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                personas.Add(new PersonaCombo
                {
                    Cedula = lector.GetString(0),
                    NombreCompleto = lector.GetString(1)
                });
            }

            return personas;
        }

        public UbicacionPersona ObtenerUbicacionPorBarrio(int idBarrio)
        {
            using var conn = _conexionDB.ObtenerConexion();

            string sql = @"
        SELECT
            p.id  AS id_provincia,
            c.id  AS id_canton,
            d.id  AS id_distrito,
            b.id  AS id_barrio
        FROM cat_barrio b
        INNER JOIN cat_distrito d ON b.id_cat_distrito = d.id
        INNER JOIN cat_canton c ON d.id_cat_canton = c.id
        INNER JOIN cat_provincia p ON c.id_cat_provincia = p.id
        WHERE b.id = @idBarrio";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@idBarrio", idBarrio);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new UbicacionPersona
                {
                    IdProvincia = reader.GetInt32(reader.GetOrdinal("id_provincia")),
                    IdCanton = reader.GetInt32(reader.GetOrdinal("id_canton")),
                    IdDistrito = reader.GetInt32(reader.GetOrdinal("id_distrito")),
                    IdBarrio = reader.GetInt32(reader.GetOrdinal("id_barrio"))
                };
            }

            return null;
        }
        public List<PersonaConRol> ListarPersonasConRol()
        {
            var personas = new List<PersonaConRol>();

            using var conexion = _conexionDB.ObtenerConexion();

            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_personas_con_rol()",
                conexion);

            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                personas.Add(new PersonaConRol
                {
                    Cedula = lector.GetString(0),
                    NombreCompleto = lector.GetString(1),
                    NombreUsuario = lector.GetString(2),
                    IdCatRol = lector.GetInt32(3),
                    Rol = lector.GetString(4)
                });
            }

            return personas;
        }


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

        public Persona? ObtenerPersonaPorCedula(string cedula)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_persona() WHERE cedula = $1", conexion);

            comando.Parameters.AddWithValue(cedula);

            using var lector = comando.ExecuteReader();
            return lector.Read() ? MapearPersona(lector) : null;
        }

        public void InsertarPersona(Persona persona, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

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

        public void ActualizarPersona(Persona persona, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

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

        public void EliminarPersona(string cedula, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

            using var comando = new NpgsqlCommand("CALL public.eliminar_persona($1)", conexion);
            comando.Parameters.AddWithValue(cedula);
            comando.ExecuteNonQuery();
        }

        private static Persona MapearPersona(NpgsqlDataReader lector) => new Persona
        {
            Cedula = lector.GetString(0),
            Nombre1 = lector.IsDBNull(1) ? string.Empty : lector.GetString(1),
            Nombre2 = lector.IsDBNull(2) ? string.Empty : lector.GetString(2),
            Apellido1 = lector.IsDBNull(3) ? string.Empty : lector.GetString(3),
            Apellido2 = lector.IsDBNull(4) ? string.Empty : lector.GetString(4),
            IdCatBarrio = lector.GetInt32(5)
        };

        // ─── TELÉFONOS DE PERSONA ────────────────────────────────────────────────

        public void InsertarTelefonoPersona(string cedula, string numero, int idCatTipoTelefono)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "CALL public.insertar_telefono_persona($1,$2,$3)", conexion);

            comando.Parameters.AddWithValue(cedula);
            comando.Parameters.AddWithValue(numero);
            comando.Parameters.AddWithValue(idCatTipoTelefono);
            comando.ExecuteNonQuery();
        }

        public List<TelefonoPersona> ListarTelefonosPersona(string cedula)
        {
            var telefonos = new List<TelefonoPersona>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_telefono_persona($1)", conexion);

            comando.Parameters.AddWithValue(cedula);

            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                telefonos.Add(new TelefonoPersona
                {
                    Codigo = lector.GetString(0),
                    Cedula = lector.GetString(1),
                    Numero = lector.GetString(2),
                    IdCatTipoTelefono = lector.GetInt32(3)
                });
            }

            return telefonos;
        }

        public void EliminarTelefonoPersona(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "CALL public.eliminar_telefono_persona($1)", conexion);

            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        // ─── CORREOS DE PERSONA ──────────────────────────────────────────────────

        public void InsertarCorreoPersona(string cedula, string correo, int idCatTipoCorreo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "CALL public.insertar_correo_electronico_persona($1,$2,$3)", conexion);

            comando.Parameters.AddWithValue(cedula);
            comando.Parameters.AddWithValue(correo);
            comando.Parameters.AddWithValue(idCatTipoCorreo);
            comando.ExecuteNonQuery();
        }

        public List<CorreoElectronicoPersona> ListarCorreosPersona(string cedula)
        {
            var correos = new List<CorreoElectronicoPersona>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_correo_electronico_persona($1)", conexion);

            comando.Parameters.AddWithValue(cedula);

            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                correos.Add(new CorreoElectronicoPersona
                {
                    Codigo = lector.GetString(0),
                    Cedula = lector.GetString(1),
                    Correo = lector.GetString(2),
                    IdCatTipoCorreo = lector.GetInt32(3)
                });
            }

            return correos;
        }

        public void EliminarCorreoPersona(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "CALL public.eliminar_correo_electronico_persona($1)", conexion);

            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        // ─── PROPIETARIOS ────────────────────────────────────────────────────────

        public List<Propietario> ListarPropietarios()
        {
            var propietarios = new List<Propietario>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_propietario()", conexion);
            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                propietarios.Add(MapearPropietario(lector));
            }

            return propietarios;
        }

        public Propietario? ObtenerPropietario(string codigo)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_propietario() WHERE codigo = $1", conexion);

            comando.Parameters.AddWithValue(codigo);

            using var lector = comando.ExecuteReader();
            return lector.Read() ? MapearPropietario(lector) : null;
        }

        public void InsertarPropietario(Propietario propietario, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

            using var comando = new NpgsqlCommand(
                "CALL public.insertar_propietario($1,$2)", conexion);

            comando.Parameters.AddWithValue(propietario.Cedula);
            comando.Parameters.AddWithValue(propietario.DescuentoProximaFacturacion);
            comando.ExecuteNonQuery();
        }

        public void ActualizarPropietario(Propietario propietario, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_propietario($1,$2,$3)", conexion);

            comando.Parameters.AddWithValue(propietario.Codigo);
            comando.Parameters.AddWithValue(propietario.Cedula);
            comando.Parameters.AddWithValue(propietario.DescuentoProximaFacturacion);
            comando.ExecuteNonQuery();
        }

        public void EliminarPropietario(string codigo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

            using var comando = new NpgsqlCommand(
                "CALL public.eliminar_propietario($1)", conexion);

            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        public void ReiniciarDescuento(string codigoPropietario)
        {
            var propietario = ObtenerPropietario(codigoPropietario);
            if (propietario == null)
            {
                return;
            }

            propietario.DescuentoProximaFacturacion = false;
            ActualizarPropietario(propietario, "sistema automatico");
        }

        private static Propietario MapearPropietario(NpgsqlDataReader lector) => new Propietario
        {
            Codigo = lector.GetString(0),
            Cedula = lector.GetString(1),
            DescuentoProximaFacturacion = lector.GetBoolean(2)
        };

        // ─── ADMINISTRADORES ─────────────────────────────────────────────────────

        public List<Administrador> ListarAdministradores()
        {
            var administradores = new List<Administrador>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_administrador()", conexion);
            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                administradores.Add(new Administrador
                {
                    Codigo = lector.GetString(0),
                    Cedula = lector.GetString(1)
                });
            }

            return administradores;
        }

        public void InsertarAdministrador(Administrador administrador, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

            using var comando = new NpgsqlCommand(
                "CALL public.insertar_administrador($1)", conexion);

            comando.Parameters.AddWithValue(administrador.Cedula);
            comando.ExecuteNonQuery();
        }

        public void ActualizarAdministrador(Administrador administrador, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_administrador($1,$2)", conexion);

            comando.Parameters.AddWithValue(administrador.Codigo);
            comando.Parameters.AddWithValue(administrador.Cedula);
            comando.ExecuteNonQuery();
        }

        public void EliminarAdministrador(string codigo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

            using var comando = new NpgsqlCommand(
                "CALL public.eliminar_administrador($1)", conexion);

            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        // ─── ENCARGADOS DE ESTABLO ───────────────────────────────────────────────

        public List<EncargadoEstablo> ListarEncargados()
        {
            var encargados = new List<EncargadoEstablo>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_encargado_establo()", conexion);
            using var lector = comando.ExecuteReader();

            while (lector.Read())
            {
                encargados.Add(new EncargadoEstablo
                {
                    Codigo = lector.GetString(0),
                    Cedula = lector.GetString(1)
                });
            }

            return encargados;
        }

        public void InsertarEncargado(EncargadoEstablo encargado, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

            using var comando = new NpgsqlCommand(
                "CALL public.insertar_encargado_establo($1)", conexion);

            comando.Parameters.AddWithValue(encargado.Cedula);
            comando.ExecuteNonQuery();
        }

        public void ActualizarEncargado(EncargadoEstablo encargado, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_encargado_establo($1,$2)", conexion);

            comando.Parameters.AddWithValue(encargado.Codigo);
            comando.Parameters.AddWithValue(encargado.Cedula);
            comando.ExecuteNonQuery();
        }

        public void EliminarEncargado(string codigo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            EstablecerUsuarioActual(conexion, usuarioActual);

            using var comando = new NpgsqlCommand(
                "CALL public.eliminar_encargado_establo($1)", conexion);

            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }
    }
}

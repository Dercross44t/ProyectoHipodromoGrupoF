using System.Collections.Generic;
using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class PersonasService
    {
        private readonly PersonasRepository _personasRepository;

        public PersonasService(PersonasRepository personasRepository)
        {
            _personasRepository = personasRepository;
        }

        // ─── PERSONAS ────────────────────────────────────────────────────────────

        public List<Persona> ListarPersonas() =>
            _personasRepository.ListarPersonas();

        public UbicacionPersona ObtenerUbicacionPorBarrio(int idBarrio)
        {
            return _personasRepository.ObtenerUbicacionPorBarrio(idBarrio);
        }

        public List<PersonaConRol> ListarPersonasConRol() =>
    _personasRepository.ListarPersonasConRol();

        public Persona? ObtenerPersonaPorCedula(string cedula) =>
            _personasRepository.ObtenerPersonaPorCedula(cedula);

        public void InsertarPersona(Persona persona, string usuarioActual) =>
            _personasRepository.InsertarPersona(persona, usuarioActual);

        public void InsertarPersona(Persona persona) =>
            _personasRepository.InsertarPersona(persona, "sistema");

        public void ActualizarPersona(Persona persona, string usuarioActual) =>
            _personasRepository.ActualizarPersona(persona, usuarioActual);

        public void EliminarPersona(string cedula, string usuarioActual) =>
            _personasRepository.EliminarPersona(cedula, usuarioActual);

        public void ActualizarPersona(Persona persona) =>
            _personasRepository.ActualizarPersona(persona, "sistema");

        public void EliminarPersona(string cedula) =>
            _personasRepository.EliminarPersona(cedula, "sistema");

        public List<PersonaCombo> ListarPersonasSinRol() =>
    _personasRepository.ListarPersonasSinRol();

        // ─── TELÉFONOS / CORREOS ─────────────────────────────────────────────────

        public void InsertarTelefonoPersona(string cedula, string numero, int tipo) =>
            _personasRepository.InsertarTelefonoPersona(cedula, numero, tipo);

        public List<TelefonoPersona> ListarTelefonosPersona(string cedula) =>
            _personasRepository.ListarTelefonosPersona(cedula);

        public void EliminarTelefonoPersona(string codigo) =>
            _personasRepository.EliminarTelefonoPersona(codigo);

        public void InsertarCorreoPersona(string cedula, string correo, int tipo) =>
            _personasRepository.InsertarCorreoPersona(cedula, correo, tipo);

        public List<CorreoElectronicoPersona> ListarCorreosPersona(string cedula) =>
            _personasRepository.ListarCorreosPersona(cedula);

        public void EliminarCorreoPersona(string codigo) =>
            _personasRepository.EliminarCorreoPersona(codigo);

        // ─── PROPIETARIOS ────────────────────────────────────────────────────────

        public List<Propietario> ListarPropietarios() =>
            _personasRepository.ListarPropietarios();

        public Propietario? ObtenerPropietario(string codigo) =>
            _personasRepository.ObtenerPropietario(codigo);

        public void InsertarPropietario(Propietario propietario, string usuarioActual) =>
            _personasRepository.InsertarPropietario(propietario, usuarioActual);

        public void InsertarPropietario(Propietario propietario) =>
            _personasRepository.InsertarPropietario(propietario, "sistema");

        public void ActualizarPropietario(Propietario propietario, string usuarioActual) =>
            _personasRepository.ActualizarPropietario(propietario, usuarioActual);

        public void EliminarPropietario(string codigo, string usuarioActual) =>
            _personasRepository.EliminarPropietario(codigo, usuarioActual);

        public void ActualizarPropietario(Propietario propietario) =>
            _personasRepository.ActualizarPropietario(propietario, "sistema");

        public void EliminarPropietario(string codigo) =>
            _personasRepository.EliminarPropietario(codigo, "sistema");

        // ─── ADMINISTRADORES ─────────────────────────────────────────────────────

        public List<Administrador> ListarAdministradores() =>
            _personasRepository.ListarAdministradores();

        public void InsertarAdministrador(Administrador administrador, string usuarioActual) =>
            _personasRepository.InsertarAdministrador(administrador, usuarioActual);

        public void InsertarAdministrador(Administrador administrador) =>
            _personasRepository.InsertarAdministrador(administrador, "sistema");

        public void ActualizarAdministrador(Administrador administrador, string usuarioActual) =>
            _personasRepository.ActualizarAdministrador(administrador, usuarioActual);

        public void EliminarAdministrador(string codigo, string usuarioActual) =>
            _personasRepository.EliminarAdministrador(codigo, usuarioActual);

        public void ActualizarAdministrador(Administrador administrador) =>
            _personasRepository.ActualizarAdministrador(administrador, "sistema");

        public void EliminarAdministrador(string codigo) =>
            _personasRepository.EliminarAdministrador(codigo, "sistema");

        // ─── ENCARGADOS DE ESTABLO ───────────────────────────────────────────────

        public List<EncargadoEstablo> ListarEncargados() =>
            _personasRepository.ListarEncargados();

        public void InsertarEncargado(EncargadoEstablo encargado, string usuarioActual) =>
            _personasRepository.InsertarEncargado(encargado, usuarioActual);

        public void InsertarEncargado(EncargadoEstablo encargado) =>
            _personasRepository.InsertarEncargado(encargado, "sistema");

        public void ActualizarEncargado(EncargadoEstablo encargado, string usuarioActual) =>
            _personasRepository.ActualizarEncargado(encargado, usuarioActual);

        public void EliminarEncargado(string codigo, string usuarioActual) =>
            _personasRepository.EliminarEncargado(codigo, usuarioActual);

        public void ActualizarEncargado(EncargadoEstablo encargado) =>
            _personasRepository.ActualizarEncargado(encargado, "sistema");

        public void EliminarEncargado(string codigo) =>
            _personasRepository.EliminarEncargado(codigo, "sistema");
    }
}

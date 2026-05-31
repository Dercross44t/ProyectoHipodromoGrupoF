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

        public List<Persona> ListarPersonas() => _personasRepository.ListarPersonas();
        public void InsertarPersona(Persona persona) => _personasRepository.InsertarPersona(persona);
        public void ActualizarPersona(Persona persona) => _personasRepository.ActualizarPersona(persona);
        public void EliminarPersona(string cedula) => _personasRepository.EliminarPersona(cedula);
        public Persona? ObtenerPersonaPorCedula(string cedula) =>
            _personasRepository.ObtenerPersonaPorCedula(cedula);

        // ─── TELÉFONOS / CORREOS ──────────────────────────────────────────────────

        public void InsertarTelefonoPersona(string cedula, string numero, int tipo) =>
            _personasRepository.InsertarTelefonoPersona(cedula, numero, tipo);
        public void EliminarTelefonoPersona(string codigo) =>
            _personasRepository.EliminarTelefonoPersona(codigo);
        public List<TelefonoPersona> ListarTelefonosPersona(string cedula) =>
            _personasRepository.ListarTelefonosPersona(cedula);

        public void InsertarCorreoPersona(string cedula, string correo, int tipo) =>
            _personasRepository.InsertarCorreoPersona(cedula, correo, tipo);
        public void EliminarCorreoPersona(string codigo) =>
            _personasRepository.EliminarCorreoPersona(codigo);
        public List<CorreoElectronicoPersona> ListarCorreosPersona(string cedula) =>
            _personasRepository.ListarCorreosPersona(cedula);

        // ─── PROPIETARIOS ────────────────────────────────────────────────────────

        public List<Propietario> ListarPropietarios() => _personasRepository.ListarPropietarios();
        public Propietario? ObtenerPropietario(string codigo) =>
            _personasRepository.ObtenerPropietario(codigo);
        public void InsertarPropietario(Propietario propietario) =>
            _personasRepository.InsertarPropietario(propietario);
        public void ActualizarPropietario(Propietario propietario) =>
            _personasRepository.ActualizarPropietario(propietario);
        public void EliminarPropietario(string codigo) =>
            _personasRepository.EliminarPropietario(codigo);

        // ─── ENCARGADOS ───────────────────────────────────────────────────────────

        public List<EncargadoEstablo> ListarEncargados() =>
            _personasRepository.ListarEncargados();
        public void InsertarEncargado(EncargadoEstablo encargado) =>
            _personasRepository.InsertarEncargado(encargado);
        public void EliminarEncargado(string codigo) =>
            _personasRepository.EliminarEncargado(codigo);
    }
}

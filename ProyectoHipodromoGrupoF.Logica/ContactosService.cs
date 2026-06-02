using System.Collections.Generic;
using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class ContactosService
    {
        private readonly ContactosRepository _contactosRepository;

        public ContactosService(ContactosRepository contactosRepository)
        {
            _contactosRepository = contactosRepository;
        }

        public List<TelefonoPersona> ListarTelefonosPersonas() =>
            _contactosRepository.ListarTelefonosPersonas();

        public List<CorreoElectronicoPersona> ListarCorreosPersonas() =>
            _contactosRepository.ListarCorreosPersonas();

        public List<TelefonoProveedor> ListarTelefonosProveedores() =>
            _contactosRepository.ListarTelefonosProveedores();

        public List<CorreoElectronicoProveedor> ListarCorreosProveedores() =>
            _contactosRepository.ListarCorreosProveedores();

        public void InsertarTelefonoPersona(TelefonoPersona telefono) =>
            _contactosRepository.InsertarTelefonoPersona(telefono);

        public void InsertarCorreoPersona(CorreoElectronicoPersona correo) =>
            _contactosRepository.InsertarCorreoPersona(correo);

        public void InsertarTelefonoProveedor(TelefonoProveedor telefono) =>
            _contactosRepository.InsertarTelefonoProveedor(telefono);

        public void InsertarCorreoProveedor(CorreoElectronicoProveedor correo) =>
            _contactosRepository.InsertarCorreoProveedor(correo);
    }
}
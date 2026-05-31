using System.Collections.Generic;
using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class EquinosService
    {
        private readonly EquinosRepository _equinosRepository;

        public EquinosService(EquinosRepository equinosRepository)
        {
            _equinosRepository = equinosRepository;
        }

        public List<Caballo> ListarCaballos() =>
            _equinosRepository.ListarCaballos();

        public List<Caballo> ListarCaballosPorPropietario(string codigoPropietario) =>
            _equinosRepository.ListarCaballosPorPropietario(codigoPropietario);

        public void InsertarCaballo(Caballo caballo, string usuarioActual) =>
            _equinosRepository.InsertarCaballo(caballo, usuarioActual);

        public void ActualizarCaballo(Caballo caballo, string usuarioActual) =>
            _equinosRepository.ActualizarCaballo(caballo, usuarioActual);

        public void EliminarCaballo(string codigo, string usuarioActual) =>
            _equinosRepository.EliminarCaballo(codigo, usuarioActual);

        public List<Establo> ListarEstablos() =>
            _equinosRepository.ListarEstablos();

        public void InsertarEstablo(Establo establo) =>
            _equinosRepository.InsertarEstablo(establo);

        public void ActualizarEstablo(Establo establo) =>
            _equinosRepository.ActualizarEstablo(establo);

        public void EliminarEstablo(string codigo) =>
            _equinosRepository.EliminarEstablo(codigo);
    }
}

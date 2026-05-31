using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Modelo;
using System.Collections.Generic;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class ProveedoresService
    {
        private readonly ProveedoresRepository _proveedoresRepository;

        public ProveedoresService(ProveedoresRepository proveedoresRepository)
        {
            _proveedoresRepository = proveedoresRepository;
        }

        public List<Proveedor> ListarProveedores()
        {
            return _proveedoresRepository.ListarProveedores();
        }

        public void InsertarProveedor(Proveedor proveedor)
        {
            _proveedoresRepository.InsertarProveedor(proveedor);
        }

        public void ActualizarProveedor(Proveedor proveedor)
        {
            _proveedoresRepository.ActualizarProveedor(proveedor);
        }

        public void EliminarProveedor(string codigo)
        {
            _proveedoresRepository.EliminarProveedor(codigo);
        }
    }
}

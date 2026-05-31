using System.Collections.Generic;
using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class CatalogosService
    {
        private readonly CatalogosRepository _repo;

        public CatalogosService(CatalogosRepository repo)
        {
            _repo = repo;
        }

        // Equinos
        public List<CatalogoItem> ListarRazas()             => _repo.ListarRazas();
        public List<CatalogoItem> ListarSexos()             => _repo.ListarSexos();
        public List<CatalogoItem> ListarEstadosCaballo()    => _repo.ListarEstadosCaballo();
        public List<CatalogoItem> ListarEstadosEstablo()    => _repo.ListarEstadosEstablo();

        // Eventos
        public List<CatalogoItem> ListarTiposCarrera()      => _repo.ListarTiposCarrera();
        public List<CatalogoItem> ListarDistancias()        => _repo.ListarDistancias();
        public List<CatalogoItem> ListarEstadosEvento()     => _repo.ListarEstadosEvento();

        // Inscripciones
        public List<CatalogoItem> ListarEstadosInscripcion()=> _repo.ListarEstadosInscripcion();

        // Facturación
        public List<CatalogoItem> ListarEstadosPago()       => _repo.ListarEstadosPago();
        public List<CatalogoItem> ListarMetodosPago()       => _repo.ListarMetodosPago();

        // Inventario
        public List<CatalogoItem> ListarTiposSuministro()  => _repo.ListarTiposSuministro();

        // Usuarios
        public List<CatalogoItem> ListarRoles()             => _repo.ListarRoles();

        // Contacto
        public List<CatalogoItem> ListarTiposTelefono()     => _repo.ListarTiposTelefono();
        public List<CatalogoItem> ListarTiposCorreo()       => _repo.ListarTiposCorreo();

        // Geografía en cascada
        public List<CatProvincia> ListarProvincias()                         => _repo.ListarProvincias();
        public List<CatCanton>    ListarCantonesPorProvincia(int idProvincia) => _repo.ListarCantonesPorProvincia(idProvincia);
        public List<CatDistrito>  ListarDistritosPorCanton(int idCanton)      => _repo.ListarDistritosPorCanton(idCanton);
        public List<CatBarrio>    ListarBarriosPorDistrito(int idDistrito)     => _repo.ListarBarriosPorDistrito(idDistrito);
    }
}

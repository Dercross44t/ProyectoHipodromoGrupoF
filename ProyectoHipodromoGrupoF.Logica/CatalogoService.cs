using ProyectoHipodromoGrupoF.AccesoADatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class CatalogoService
    {
        private readonly CatalogoRepository _catalogoRepository;

        public CatalogoService(CatalogoRepository catalogoRepository)
        {
            _catalogoRepository = catalogoRepository;
        }

        public Dictionary<string, DataTable> ListarBitacorasTrimestrales() =>
            _catalogoRepository.ListarBitacorasTrimestrales();
    }
}

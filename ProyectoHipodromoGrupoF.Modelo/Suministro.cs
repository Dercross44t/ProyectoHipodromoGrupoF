using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class Suministro
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string Nombre { get; set; } // máximo 50 caracteres
        public string CodigoProveedor { get; set; } // máximo 7 caracteres, FK hacia Proveedor
        public int CantidadDisponible { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int IdCatTipoSuministro { get; set; } // FK hacia catálogo de tipo suministro
    }
}

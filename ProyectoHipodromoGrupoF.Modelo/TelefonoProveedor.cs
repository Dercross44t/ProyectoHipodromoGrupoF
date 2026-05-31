using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class TelefonoProveedor
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string CodigoProveedor { get; set; } // máximo 7 caracteres, FK hacia Proveedor
        public string Numero { get; set; } // 8 dígitos exactos
        public int IdCatTipoTelefono { get; set; } // FK hacia catálogo de tipo teléfono
    }
}

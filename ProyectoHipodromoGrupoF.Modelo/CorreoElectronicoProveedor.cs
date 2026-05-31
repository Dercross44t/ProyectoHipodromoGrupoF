using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{

    public class CorreoElectronicoProveedor
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string CodigoProveedor { get; set; } // máximo 7 caracteres, FK hacia Proveedor
        public string Correo { get; set; } // máximo 100 caracteres
        public int IdCatTipoCorreo { get; set; } // FK hacia catálogo de tipo correo
    }

}

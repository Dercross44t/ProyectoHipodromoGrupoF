using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class CorreoElectronicoPersona
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string Cedula { get; set; } // 9 dígitos exactos, FK hacia Persona
        public string Correo { get; set; } // máximo 100 caracteres
        public int IdCatTipoCorreo { get; set; } // FK hacia catálogo de tipo correo
    }
}

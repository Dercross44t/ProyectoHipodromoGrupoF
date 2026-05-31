using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class TelefonoPersona
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string Cedula { get; set; } // 9 dígitos exactos, FK hacia Persona
        public string Numero { get; set; } // 8 dígitos exactos
        public int IdCatTipoTelefono { get; set; } // FK hacia catálogo de tipo teléfono
    }
}

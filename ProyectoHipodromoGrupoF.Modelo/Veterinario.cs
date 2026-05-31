using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{

    public class Veterinario
    {
        public string Codigo { get; set; } // máximo 10 caracteres, se genera automáticamente
        public string NumeroColegiado { get; set; } // 9 dígitos exactos
        public string Cedula { get; set; } // 9 dígitos exactos, FK hacia Persona
        public string Especialidad { get; set; } // máximo 50 caracteres
    }

}

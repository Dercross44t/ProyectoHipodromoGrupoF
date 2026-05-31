using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class Administrador
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string Cedula { get; set; } // 9 dígitos exactos, FK hacia Persona
    }
}

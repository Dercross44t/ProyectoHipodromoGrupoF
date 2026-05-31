using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class Usuario
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string Cedula { get; set; } // 9 dígitos exactos, FK hacia Persona
        public string Nombre { get; set; } // máximo 20 caracteres
        public string Contrasena { get; set; } // máximo 50 caracteres
        public int IdCatRol { get; set; } // FK hacia catálogo de rol
    }
}

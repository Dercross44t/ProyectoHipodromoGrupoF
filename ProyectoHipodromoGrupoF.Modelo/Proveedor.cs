using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class Proveedor
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string Nombre { get; set; } // máximo 50 caracteres
    }
}

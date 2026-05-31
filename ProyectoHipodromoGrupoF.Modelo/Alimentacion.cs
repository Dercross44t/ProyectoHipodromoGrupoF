using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class Alimentacion
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string CodigoCaballo { get; set; } // máximo 7 caracteres, FK hacia Caballo
        public string CodigoSuministro { get; set; } // máximo 7 caracteres, FK hacia Suministro
        public DateTime Fecha { get; set; }
        public int Cantidad { get; set; }
    }
}

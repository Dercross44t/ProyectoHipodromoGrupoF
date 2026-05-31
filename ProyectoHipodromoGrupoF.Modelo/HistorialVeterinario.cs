using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class HistorialVeterinario
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string CodigoCaballo { get; set; } // máximo 7 caracteres, FK hacia Caballo
        public string CodigoVeterinario { get; set; } // máximo 7 caracteres, FK hacia Veterinario
        public string Diagnostico { get; set; } // máximo 200 caracteres
        public string Tratamiento { get; set; } // máximo 200 caracteres
        public DateTime FechaRevision { get; set; }
        public DateTime FechaVencimientoCertificacion { get; set; }
    }
}

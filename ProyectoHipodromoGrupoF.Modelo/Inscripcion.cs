using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class Inscripcion
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string CodigoEvento { get; set; } // máximo 7 caracteres, FK hacia Evento
        public string CodigoCaballo { get; set; } // máximo 7 caracteres, FK hacia Caballo
        public DateTime Fecha { get; set; }
        public int IdCatEstadoInscripcion { get; set; } // FK hacia catálogo de estado inscripción
    }
}

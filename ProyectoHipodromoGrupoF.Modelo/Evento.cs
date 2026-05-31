using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class Evento
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string Nombre { get; set; } // máximo 50 caracteres
        public DateTime Fecha { get; set; }
        public double PremioTotal { get; set; }
        public int IdCatEstadoEvento { get; set; } // FK hacia catálogo de estado evento
        public string CodigoCarrera { get; set; } // máximo 7 caracteres, FK hacia Carrera
    }
}

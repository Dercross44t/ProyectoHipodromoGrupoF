using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class CaballoInscripcion
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool PuedeInscribirse { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}

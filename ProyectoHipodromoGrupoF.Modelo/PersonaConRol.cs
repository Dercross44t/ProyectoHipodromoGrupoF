using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class PersonaConRol
    {
        public string Cedula { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public int IdCatRol { get; set; }
        public string Rol { get; set; } = string.Empty;
    }
}

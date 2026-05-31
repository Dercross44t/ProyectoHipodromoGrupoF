using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class Establo
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string Nombre { get; set; } = string.Empty; // nombre del establo
        public int IdCatBarrio { get; set; } // FK hacia catálogo de barrio
        public int Capacidad { get; set; } // cantidad máxima de caballos
        public int IdCatEstadoEstablo { get; set; } // FK hacia catálogo de estado establo
        public string? CodigoEncargadoEstablo { get; set; } // FK hacia encargado establo
    }
}

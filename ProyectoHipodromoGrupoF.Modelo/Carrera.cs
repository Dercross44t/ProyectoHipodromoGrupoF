using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class Carrera
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string Nombre { get; set; } // máximo 100 caracteres
        public int IdCatTipoCarrera { get; set; } // FK hacia catálogo de tipo carrera
        public int IdCatDistancia { get; set; } // FK hacia catálogo de distancia
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class Caballo
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string CodigoPropietario { get; set; } // máximo 7 caracteres, FK hacia Propietario
        public string CodigoEstablo { get; set; } // máximo 7 caracteres, FK hacia Establo
        public string Nombre { get; set; } // máximo 50 caracteres
        public DateOnly FechaNacimiento { get; set; }
        public int IdCatSexo { get; set; } // FK hacia catálogo de sexo
        public int IdCatRaza { get; set; } // FK hacia catálogo de raza
        public double Peso { get; set; }
        public int IdCatEstadoCaballo { get; set; } // FK hacia catálogo de estado caballo
    }
}

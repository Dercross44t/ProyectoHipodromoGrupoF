using System;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class ResultadoCarrera
    {
        public string? Codigo { get; set; }
        public required string CodigoInscripcion { get; set; }
        public int Posicion { get; set; }
        public string? Tiempo { get; set; }
        public double PremioObtenido { get; set; }
    }
}

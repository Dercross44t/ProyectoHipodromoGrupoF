using System;

namespace ProyectoHipodromoGrupoF.Modelo
{
    /// <summary>
    /// Representa una alerta de certificación veterinaria vencida o próxima a vencer.
    /// Se usa en el Dashboard y el badge del menú de navegación.
    /// </summary>
    public class AlertaVeterinaria
    {
        public string CodigoCaballo { get; set; } = string.Empty;
        public string NombreCaballo { get; set; } = string.Empty;
        public string CodigoVeterinario { get; set; } = string.Empty;
        public DateTime FechaRevision { get; set; }
        public DateTime FechaVencimientoCertificacion { get; set; }

        /// <summary>Días restantes (negativo = ya vencida).</summary>
        public int DiasRestantes { get; set; }

        public bool EstaVencida => DiasRestantes < 0;
        public bool EsUrgente   => DiasRestantes >= 0 && DiasRestantes <= 7;
    }
}

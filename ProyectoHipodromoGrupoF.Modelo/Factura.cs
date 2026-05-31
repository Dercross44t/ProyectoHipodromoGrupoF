using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class Factura
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string CodigoPropietario { get; set; } // máximo 7 caracteres, FK hacia Propietario
        public string CodigoEvento { get; set; } // máximo 7 caracteres, FK hacia Evento
        public double Subtotal { get; set; }
        public double Descuento { get; set; }
        public double Impuestos { get; set; }
        public double Total { get; set; }
        public int IdCatEstadoPago { get; set; } // FK hacia catálogo de estado pago
    }
}

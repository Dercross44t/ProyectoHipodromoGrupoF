using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoHipodromoGrupoF.Modelo
{
    public class HistorialTransaccion
    {
        public string Codigo { get; set; } // máximo 7 caracteres, se genera automáticamente
        public string CodigoFactura { get; set; } // máximo 7 caracteres, FK hacia Factura
        public DateTime Fecha { get; set; }
        public double Monto { get; set; }
        public int IdCatMetodoPago { get; set; } // FK hacia catálogo de método de pago
    }
}

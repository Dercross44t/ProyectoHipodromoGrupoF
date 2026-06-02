using System;
using System.Collections.Generic;
using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.Logica
{
    public class FacturacionService
    {
        private readonly FacturacionRepository _facturacionRepository;
        private readonly PersonasRepository    _personasRepository;

        // IVA vigente en Costa Rica según Ley 9635 (13%)
        private const double TasaIVA = 0.13;

        public FacturacionService(
            FacturacionRepository facturacionRepository,
            PersonasRepository personasRepository)
        {
            _facturacionRepository = facturacionRepository;
            _personasRepository    = personasRepository;
        }

        // ─── FACTURAS ────────────────────────────────────────────────────────────

        public List<Factura> ListarFacturas() => _facturacionRepository.ListarFacturas();

        public List<Factura> ListarFacturasPorPropietario(string codigoPropietario) =>
            _facturacionRepository.ListarFacturasPorPropietario(codigoPropietario);

        public List<HistorialTransaccion> ListarTransacciones() =>
            _facturacionRepository.ListarTransacciones();

        public void GenerarFacturaPorInscripcion(string codigoPropietario, string codigoEvento, string usuarioActual)
        {
            var factura = new Factura
            {
                CodigoPropietario = codigoPropietario,
                CodigoEvento = codigoEvento,
                Subtotal = 30000
            };

            InsertarFactura(factura, usuarioActual);
        }
        public void InsertarFactura(Factura factura, string usuarioActual)
        {
            var propietario = _personasRepository.ObtenerPropietario(factura.CodigoPropietario);
            if (propietario != null && propietario.DescuentoProximaFacturacion)
                factura.Descuento = Math.Round(factura.Subtotal * 0.10, 2);
            else
                factura.Descuento = 0;

            double montoNeto  = factura.Subtotal - factura.Descuento;
            factura.Impuestos = Math.Round(montoNeto * TasaIVA, 2);
            factura.Total     = Math.Round(montoNeto + factura.Impuestos, 2);
            factura.IdCatEstadoPago = 1; // Pendiente

            _facturacionRepository.InsertarFactura(factura, usuarioActual);

            if (propietario != null && propietario.DescuentoProximaFacturacion)
                _personasRepository.ReiniciarDescuento(factura.CodigoPropietario);

            if (propietario != null)
                _facturacionRepository.EvaluarDescuentoClienteFrecuente(
                    factura.CodigoPropietario, propietario.Cedula);
        }

        public void ActualizarFactura(Factura factura, string usuarioActual) =>
            _facturacionRepository.ActualizarFactura(factura, usuarioActual);

        public void EliminarFactura(string codigo, string usuarioActual) =>
            _facturacionRepository.EliminarFactura(codigo, usuarioActual);

        public void PagarFactura(string codigoFactura, double montoPagado, int idCatMetodoPago, string usuarioActual)
        {
            var factura = _facturacionRepository.ObtenerFacturaPorCodigo(codigoFactura);

            if (factura == null)
                throw new Exception("No se encontró la factura.");

            if (factura.IdCatEstadoPago != 1)
                throw new Exception("Solo se pueden pagar facturas pendientes.");

            if (montoPagado <= 0)
                throw new Exception("El monto pagado debe ser mayor a cero.");

            if (montoPagado > factura.Total)
                throw new Exception("El monto pagado no puede ser mayor al total de la factura.");

            _facturacionRepository.InsertarTransaccion(new HistorialTransaccion
            {
                CodigoFactura = factura.Codigo,
                Fecha = DateTime.Today,
                Monto = montoPagado,
                IdCatMetodoPago = idCatMetodoPago
            });

            if (montoPagado == factura.Total)
            {
                factura.IdCatEstadoPago = 2; // Pagado
                _facturacionRepository.ActualizarFactura(factura, usuarioActual);
                return;
            }

            factura.IdCatEstadoPago = 5; // Parcial
            _facturacionRepository.ActualizarFactura(factura, usuarioActual);

            double montoPendiente = factura.Total - montoPagado;

            var nuevaFactura = new Factura
            {
                CodigoPropietario = factura.CodigoPropietario,
                CodigoEvento = factura.CodigoEvento,
                Subtotal = montoPendiente,
                Descuento = 0,
                Impuestos = 0,
                Total = montoPendiente,
                IdCatEstadoPago = 1 // Pendiente
            };

            _facturacionRepository.InsertarFactura(nuevaFactura, usuarioActual);
        }

        // ─── TRANSACCIONES ────────────────────────────────────────────────────────

        public void InsertarTransaccion(HistorialTransaccion transaccion) =>
            _facturacionRepository.InsertarTransaccion(transaccion);

        public double ObtenerTotalFactura(string codigoFactura) =>
            _facturacionRepository.ObtenerTotalFactura(codigoFactura);

        // ─── STORED PROCEDURES ────────────────────────────────────────────────────

        /// <summary>
        /// Genera factura completa directamente desde el SP en BD
        /// (IVA 13% + comisión 5% + descuento automático).
        /// </summary>
        public void GenerarFacturaAutomatica(string codigoPropietario, string codigoEvento, double precio) =>
            _facturacionRepository.GenerarFacturaAutomatica(codigoPropietario, codigoEvento, precio);

        /// <summary>Evalúa todos los propietarios y activa descuento si superaron ₡500k.</summary>
        public void EjecutarEvaluacionPropietariosFrecuentes() =>
            _facturacionRepository.EjecutarEvaluacionPropietariosFrecuentes();

        /// <summary>Calcula distribución de premios del evento (60%/25%/15%).</summary>
        public void CalcularPremiosEvento(string codigoEvento) =>
            _facturacionRepository.CalcularPremiosEvento(codigoEvento);

        // ─── KPIs DASHBOARD ──────────────────────────────────────────────────────

        public (double IngresosMes, double CuentasPorCobrar, int FacturasPagadas, int FacturasPendientes)
            ObtenerKPIs() => _facturacionRepository.ObtenerKPIs();
    }
}

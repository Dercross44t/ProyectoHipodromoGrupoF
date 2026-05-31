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

        /// <summary>
        /// Genera una factura aplicando la lógica de negocio completa:
        /// 1. Verifica si el propietario tiene descuento activo (10%).
        /// 2. Calcula IVA 13% (legislación CR).
        /// 3. Inserta la factura.
        /// 4. Reinicia el flag de descuento si fue usado.
        /// 5. Evalúa si el propietario supera ₡500k en 6 meses para activar próximo descuento.
        /// </summary>
        public void InsertarFactura(Factura factura)
        {
            var propietario = _personasRepository.ObtenerPropietario(factura.CodigoPropietario);
            if (propietario != null && propietario.DescuentoProximaFacturacion)
                factura.Descuento = Math.Round(factura.Subtotal * 0.10, 2);
            else
                factura.Descuento = 0;

            double montoNeto  = factura.Subtotal - factura.Descuento;
            factura.Impuestos = Math.Round(montoNeto * TasaIVA, 2);
            factura.Total     = Math.Round(montoNeto + factura.Impuestos, 2);
            factura.IdCatEstadoPago = 2; // Pendiente

            _facturacionRepository.InsertarFactura(factura);

            if (propietario != null && propietario.DescuentoProximaFacturacion)
                _personasRepository.ReiniciarDescuento(factura.CodigoPropietario);

            if (propietario != null)
                _facturacionRepository.EvaluarDescuentoClienteFrecuente(
                    factura.CodigoPropietario, propietario.Cedula);
        }

        public void ActualizarFactura(Factura factura) =>
            _facturacionRepository.ActualizarFactura(factura);

        public void EliminarFactura(string codigo) =>
            _facturacionRepository.EliminarFactura(codigo);

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

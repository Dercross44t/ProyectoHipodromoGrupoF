using System;
using System.Collections.Generic;
using Npgsql;
using ProyectoHipodromoGrupoF.Modelo;

namespace ProyectoHipodromoGrupoF.AccesoADatos
{
    public class FacturacionRepository
    {
        private readonly ConexionDB _conexionDB;

        public FacturacionRepository(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        // ─── FACTURAS ────────────────────────────────────────────────────────────

        public List<Factura> ListarFacturas()
        {
            var facturas = new List<Factura>();
            using var conexion = _conexionDB.ObtenerConexion();
            // Ahora usamos la función listar_factura() creada en la BD
            using var comando = new NpgsqlCommand("SELECT * FROM public.listar_factura()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                facturas.Add(MapearFactura(lector));
            }
            return facturas;
        }

        public List<Factura> ListarFacturasPorPropietario(string codigoPropietario)
        {
            var facturas = new List<Factura>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_facturas_por_propietario($1)", conexion);
            comando.Parameters.AddWithValue(codigoPropietario);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                facturas.Add(MapearFactura(lector));
            }
            return facturas;
        }

        public void InsertarFactura(Factura factura, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual ?? "usuario_desconocido");
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.insertar_factura($1,$2,$3,$4,$5,$6,$7)", conexion);
            comando.Parameters.AddWithValue(factura.CodigoPropietario);
            comando.Parameters.AddWithValue(
                string.IsNullOrEmpty(factura.CodigoEvento) ? DBNull.Value : (object)factura.CodigoEvento);
            comando.Parameters.AddWithValue(factura.Subtotal);
            comando.Parameters.AddWithValue(factura.Descuento);
            comando.Parameters.AddWithValue(factura.Impuestos);
            comando.Parameters.AddWithValue(factura.Total);
            comando.Parameters.AddWithValue(factura.IdCatEstadoPago);
            comando.ExecuteNonQuery();
        }

        public void ActualizarFactura(Factura factura, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual ?? "usuario_desconocido");
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand(
                "CALL public.actualizar_factura($1,$2,$3,$4,$5,$6,$7,$8)", conexion);
            comando.Parameters.AddWithValue(factura.Codigo);
            comando.Parameters.AddWithValue(factura.CodigoPropietario);
            comando.Parameters.AddWithValue(
                string.IsNullOrEmpty(factura.CodigoEvento) ? DBNull.Value : (object)factura.CodigoEvento);
            comando.Parameters.AddWithValue(factura.Subtotal);
            comando.Parameters.AddWithValue(factura.Descuento);
            comando.Parameters.AddWithValue(factura.Impuestos);
            comando.Parameters.AddWithValue(factura.Total);
            comando.Parameters.AddWithValue(factura.IdCatEstadoPago);
            comando.ExecuteNonQuery();
        }

        public void EliminarFactura(string codigo, string usuarioActual)
        {
            using var conexion = _conexionDB.ObtenerConexion();

            using var comandoUsuario = new NpgsqlCommand(
                "CALL public.establecer_usuario_actual($1)", conexion);

            comandoUsuario.Parameters.AddWithValue(usuarioActual ?? "usuario_desconocido");
            comandoUsuario.ExecuteNonQuery();

            using var comando = new NpgsqlCommand("CALL public.eliminar_factura($1)", conexion);
            comando.Parameters.AddWithValue(codigo);
            comando.ExecuteNonQuery();
        }

        // ─── HISTORIAL DE TRANSACCIONES ──────────────────────────────────────────

        public List<HistorialTransaccion> ListarTransacciones()
        {
            var lista = new List<HistorialTransaccion>();
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                "SELECT * FROM public.listar_historial_transaccion()", conexion);
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                lista.Add(new HistorialTransaccion
                {
                    Codigo          = lector.GetString(0),
                    CodigoFactura   = lector.GetString(1),
                    Fecha           = lector.GetDateTime(2),
                    Monto           = lector.GetDouble(3),
                    IdCatMetodoPago = lector.GetInt32(4)
                });
            }
            return lista;
        }

        // ─── LÓGICA DE DESCUENTO CLIENTE FRECUENTE (5% rúbrica) ─────────────────

        /// <summary>
        /// Calcula el total facturado por el propietario en los últimos 6 meses.
        /// Si supera ₡500.000, activa el flag de descuento para la próxima factura.
        /// </summary>
        public void EvaluarDescuentoClienteFrecuente(string codigoPropietario, string cedula)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmdTotal = new NpgsqlCommand(
                "SELECT public.calcular_total_facturado_6_meses($1)", conexion);
            cmdTotal.Parameters.AddWithValue(codigoPropietario);
            var totalObj = cmdTotal.ExecuteScalar();
            double total = totalObj == null || totalObj == DBNull.Value
                           ? 0 : Convert.ToDouble(totalObj);

            if (total > 500000)
            {
                // Activar descuento para próxima factura
                using var cmdUpdate = new NpgsqlCommand(
                    "CALL public.actualizar_propietario($1,$2,$3)", conexion);
                cmdUpdate.Parameters.AddWithValue(codigoPropietario);
                cmdUpdate.Parameters.AddWithValue(cedula);
                cmdUpdate.Parameters.AddWithValue(true);
                cmdUpdate.ExecuteNonQuery();
            }
        }

        // ─── KPIs DASHBOARD ──────────────────────────────────────────────────────

        public (double IngresosMes, double CuentasPorCobrar, int FacturasPagadas, int FacturasPendientes)
            ObtenerKPIs()
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var comando = new NpgsqlCommand(
                @"SELECT
                    COALESCE(SUM(CASE WHEN id_cat_estado_pago = 1 THEN total ELSE 0 END), 0)  AS ingresos_mes,
                    COALESCE(SUM(CASE WHEN id_cat_estado_pago = 2 THEN total ELSE 0 END), 0)  AS cuentas_cobrar,
                    COUNT(CASE WHEN id_cat_estado_pago = 1 THEN 1 END)                        AS pagadas,
                    COUNT(CASE WHEN id_cat_estado_pago = 2 THEN 1 END)                        AS pendientes
                  FROM public.factura", conexion);
            using var lector = comando.ExecuteReader();
            if (lector.Read())
            {
                return (
                    lector.GetDouble(0),
                    lector.GetDouble(1),
                    lector.GetInt32(2),
                    lector.GetInt32(3)
                );
            }
            return (0, 0, 0, 0);
        }

        private static Factura MapearFactura(NpgsqlDataReader lector) => new Factura
        {
            Codigo             = lector.GetString(0),
            CodigoPropietario  = lector.IsDBNull(1) ? string.Empty : lector.GetString(1),
            CodigoEvento       = lector.IsDBNull(2) ? string.Empty : lector.GetString(2),
            Subtotal           = lector.GetDouble(3),
            Descuento          = lector.GetDouble(4),
            Impuestos          = lector.GetDouble(5),
            Total              = lector.GetDouble(6),
            IdCatEstadoPago    = lector.GetInt32(7)
        };

        // ─── HISTORIAL DE TRANSACCIONES — INSERT ─────────────────────────────────

        public void InsertarTransaccion(HistorialTransaccion transaccion)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            // insertar_historial_transaccion(codigo_factura, fecha, monto, id_cat_metodo_pago)
            using var cmd = new NpgsqlCommand(
                "CALL public.insertar_historial_transaccion($1,$2,$3,$4)", conexion);
            cmd.Parameters.AddWithValue(transaccion.CodigoFactura);
            cmd.Parameters.AddWithValue(DateOnly.FromDateTime(transaccion.Fecha));
            cmd.Parameters.AddWithValue(transaccion.Monto);
            cmd.Parameters.AddWithValue(transaccion.IdCatMetodoPago);
            cmd.ExecuteNonQuery();
        }

        // ─── STORED PROCEDURES ────────────────────────────────────────────────────

        /// <summary>Genera factura completa con IVA 13%, comisión 5% y descuento automático.</summary>
        public void GenerarFacturaAutomatica(string codigoPropietario, string codigoEvento, double precio)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("CALL public.sp_generar_factura($1,$2,$3)", conexion);
            cmd.Parameters.AddWithValue(codigoPropietario);
            cmd.Parameters.AddWithValue(codigoEvento);
            cmd.Parameters.AddWithValue(precio);
            cmd.ExecuteNonQuery();
        }

        /// <summary>Evalúa todos los propietarios y activa descuento si superaron ₡500k en 6 meses.</summary>
        public void EjecutarEvaluacionPropietariosFrecuentes()
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand(
                "CALL public.sp_aplicar_descuento_propietarios_frecuentes()", conexion);
            cmd.ExecuteNonQuery();
        }

        /// <summary>Calcula distribución de premios del evento (60/25/15%).</summary>
        public void CalcularPremiosEvento(string codigoEvento)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand("CALL public.sp_calcular_premios_evento($1)", conexion);
            cmd.Parameters.AddWithValue(codigoEvento);
            cmd.ExecuteNonQuery();
        }

        /// <summary>Obtiene el total de una factura por código (para pre-cargar en transacción).</summary>
        public double ObtenerTotalFactura(string codigoFactura)
        {
            using var conexion = _conexionDB.ObtenerConexion();
            using var cmd = new NpgsqlCommand(
                "SELECT total FROM public.factura WHERE codigo = $1", conexion);
            cmd.Parameters.AddWithValue(codigoFactura);
            var result = cmd.ExecuteScalar();
            return result == null || result == DBNull.Value ? 0 : Convert.ToDouble(result);
        }
    }
}

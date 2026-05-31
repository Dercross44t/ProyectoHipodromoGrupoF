-- =============================================================
-- COMPLEMENTO PARA LA APLICACION MVC
-- Hipodromo Nacional - Validaciones por usuario/rol
-- Ejecutar en PostgreSQL/Supabase solo si estas funciones no existen
-- o si quieres sobrescribirlas con la version compatible con la app.
-- =============================================================

-- 1. Obtener contrasena actual de usuario
CREATE OR REPLACE FUNCTION public.obtener_contrasena_usuario(p_codigo character varying)
RETURNS character varying
LANGUAGE sql
AS $$
    SELECT u.contrasena
    FROM public.usuario u
    WHERE u.codigo = p_codigo;
$$;

-- 2. Obtener codigo de propietario desde la cedula de persona/usuario
CREATE OR REPLACE FUNCTION public.obtener_codigo_propietario_por_cedula(p_cedula character varying)
RETURNS character varying
LANGUAGE sql
AS $$
    SELECT p.codigo
    FROM public.propietario p
    WHERE p.cedula = p_cedula
    LIMIT 1;
$$;

-- 3. Listar caballos de un propietario especifico
CREATE OR REPLACE FUNCTION public.listar_caballos_por_propietario(p_codigo_propietario character varying)
RETURNS TABLE (
    codigo character varying,
    codigo_propietario character varying,
    codigo_establo character varying,
    nombre character varying,
    fecha_nacimiento date,
    id_cat_sexo integer,
    id_cat_raza integer,
    peso double precision,
    id_cat_estado_caballo integer
)
LANGUAGE sql
AS $$
    SELECT
        c.codigo,
        c.codigo_propietario,
        c.codigo_establo,
        c.nombre,
        c.fecha_nacimiento,
        c.id_cat_sexo,
        c.id_cat_raza,
        c.peso,
        c.id_cat_estado_caballo
    FROM public.caballo c
    WHERE c.codigo_propietario = p_codigo_propietario;
$$;

-- 4. Listar inscripciones de los caballos de un propietario especifico
CREATE OR REPLACE FUNCTION public.listar_inscripciones_por_propietario(p_codigo_propietario character varying)
RETURNS TABLE (
    codigo character varying,
    codigo_evento character varying,
    codigo_caballo character varying,
    fecha date,
    id_cat_estado_inscripcion integer
)
LANGUAGE sql
AS $$
    SELECT
        i.codigo,
        i.codigo_evento,
        i.codigo_caballo,
        i.fecha,
        i.id_cat_estado_inscripcion
    FROM public.inscripcion i
    INNER JOIN public.caballo c
        ON c.codigo = i.codigo_caballo
    WHERE c.codigo_propietario = p_codigo_propietario;
$$;

-- 5. Listar facturas de un propietario especifico
CREATE OR REPLACE FUNCTION public.listar_facturas_por_propietario(p_codigo_propietario character varying)
RETURNS TABLE (
    codigo character varying,
    codigo_propietario character varying,
    codigo_evento character varying,
    subtotal double precision,
    descuento double precision,
    impuestos double precision,
    total double precision,
    id_cat_estado_pago integer
)
LANGUAGE sql
AS $$
    SELECT
        f.codigo,
        f.codigo_propietario,
        f.codigo_evento,
        f.subtotal,
        f.descuento,
        f.impuestos,
        f.total,
        f.id_cat_estado_pago
    FROM public.factura f
    WHERE f.codigo_propietario = p_codigo_propietario;
$$;

-- 6. Alertas de certificacion veterinaria vencida o por vencer
CREATE OR REPLACE FUNCTION public.listar_alertas_certificacion_veterinaria()
RETURNS TABLE (
    codigo_historial character varying,
    codigo_caballo character varying,
    nombre_caballo character varying,
    codigo_propietario character varying,
    fecha_vencimiento_certificacion date,
    dias_restantes integer
)
LANGUAGE sql
AS $$
    SELECT
        hv.codigo AS codigo_historial,
        hv.codigo_caballo,
        c.nombre AS nombre_caballo,
        c.codigo_propietario,
        hv.fecha_vencimiento_certificacion,
        (hv.fecha_vencimiento_certificacion - CURRENT_DATE)::integer AS dias_restantes
    FROM public.historial_veterinario hv
    INNER JOIN public.caballo c
        ON c.codigo = hv.codigo_caballo
    WHERE hv.fecha_vencimiento_certificacion <= CURRENT_DATE + INTERVAL '30 days'
    ORDER BY hv.fecha_vencimiento_certificacion ASC;
$$;

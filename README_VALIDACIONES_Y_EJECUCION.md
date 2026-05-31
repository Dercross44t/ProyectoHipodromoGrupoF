# Proyecto Hipodromo - Version con validaciones por rol

## Proyecto que se debe iniciar

En Visual Studio Community abre:

`ProyectoHipodromoGrupoF.slnx`

Luego selecciona como proyecto de inicio:

`ProyectoHipodromoGrupoF.UI`

Ejecuta con perfil `https`.

## Archivo de conexion

La conexion a Supabase/PostgreSQL se configura en:

`ProyectoHipodromoGrupoF.UI/appsettings.json`

Cadena esperada:

```json
"ConnectionStrings": {
  "HipodromoDB": "Host=TU_HOST_SUPABASE;Port=5432;Database=postgres;Username=postgres;Password=TU_PASSWORD;SSL Mode=Require;Trust Server Certificate=true;Include Error Detail=true"
}
```

## Archivos principales de conexion

- `ProyectoHipodromoGrupoF.AccesoADatos/ConexionDB.cs`
- `ProyectoHipodromoGrupoF.AccesoADatos/DBContext.cs`

La aplicacion usa repositorios con Npgsql. Aunque el proyecto incluye paquetes de Entity Framework, esta version trabaja principalmente llamando funciones y procedimientos almacenados de PostgreSQL, que es lo que pide la rubrica.

## Login y permisos por rol

La autenticacion se hace contra la tabla `usuario` de la base de datos, no contra usuarios de pgAdmin.

Tabla usada:

`public.usuario`

Roles esperados:

- `1` = Propietario
- `2` = Administrador
- `3` = Encargado de establo
- `4` = Veterinario

## Reglas de acceso implementadas

Administrador:
- Acceso general al dashboard.
- Personas, usuarios, propietarios, veterinarios.
- Eventos, carreras, resultados, facturacion, proveedores.
- Gestion administrativa general.

Propietario:
- Puede ver sus caballos.
- Puede ver sus inscripciones.
- Puede ver sus facturas.
- Puede consultar eventos y resultados.
- No puede administrar datos de otros propietarios.

Encargado de establo:
- Puede gestionar establos, inventario y alimentacion.
- Puede consultar informacion equina necesaria para su trabajo.
- No puede administrar facturacion ni usuarios.

Veterinario:
- Puede gestionar historial veterinario.
- Puede consultar caballos y alertas veterinarias.
- No puede administrar facturacion ni usuarios.

## Archivos donde estan las validaciones

- `ProyectoHipodromoGrupoF.UI/Program.cs`
- `ProyectoHipodromoGrupoF.UI/Controllers/AuthController.cs`
- `ProyectoHipodromoGrupoF.UI/Controllers/EquinosController.cs`
- `ProyectoHipodromoGrupoF.UI/Controllers/EventosController.cs`
- `ProyectoHipodromoGrupoF.UI/Controllers/FacturacionController.cs`
- `ProyectoHipodromoGrupoF.UI/Controllers/InscripcionesController.cs`
- `ProyectoHipodromoGrupoF.UI/Controllers/InventarioController.cs`
- `ProyectoHipodromoGrupoF.UI/Controllers/PersonasController.cs`
- `ProyectoHipodromoGrupoF.UI/Controllers/ProveedoresController.cs`
- `ProyectoHipodromoGrupoF.UI/Controllers/ResultadosController.cs`
- `ProyectoHipodromoGrupoF.UI/Views/Shared/_Layout.cshtml`

## Script adicional para la base de datos

Si al ejecutar el programa falta alguna funcion usada por la app, ejecuta:

`SQL/Complemento_App_Validaciones.sql`

Este archivo agrega funciones de apoyo para:

- Buscar codigo de propietario por cedula.
- Listar caballos de un propietario.
- Listar inscripciones de un propietario.
- Listar facturas de un propietario.
- Listar alertas de certificacion veterinaria.
- Obtener contrasena actual para actualizacion de usuario.

## Nota importante

Las contrasenas estan en texto plano porque asi se venia trabajando en la base del proyecto. Para un sistema real se debe usar hash de contrasena, pero para el alcance academico se mantuvo compatible con la tabla actual.

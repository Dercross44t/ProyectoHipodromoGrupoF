# ProyectoHipodromoGrupoF - Versión limpia para Visual Studio Community

## Qué contiene

Proyecto ASP.NET Core MVC en C# con estructura por capas:

- `ProyectoHipodromoGrupoF.Modelo`: clases del sistema.
- `ProyectoHipodromoGrupoF.AccesoADatos`: conexión y repositorios usando Npgsql.
- `ProyectoHipodromoGrupoF.Logica`: servicios.
- `ProyectoHipodromoGrupoF.UI`: aplicación MVC, controladores y vistas.

El proyecto original estaba en `net10.0`. Se cambió a `net8.0` para que sea más compatible con Visual Studio Community normal.

## Cómo abrirlo

1. Abre Visual Studio Community.
2. Selecciona **Open a project or solution**.
3. Abre `ProyectoHipodromoGrupoF.slnx`.
4. Restaura los paquetes NuGet si Visual Studio lo solicita.
5. Configura la cadena de conexión en:

```json
ProyectoHipodromoGrupoF.UI/appsettings.json
```

## Cadena para Supabase/PostgreSQL

Usa una cadena similar a esta:

```json
"Host=TU_HOST_SUPABASE;Port=5432;Database=postgres;Username=postgres;Password=TU_PASSWORD;SSL Mode=Require;Trust Server Certificate=true;Include Error Detail=true"
```

Si usas el Transaction Pooler de Supabase, normalmente el puerto puede ser `6543` y el usuario suele tener formato `postgres.xxxxx`.

## Importante

El proyecto llama funciones y procedimientos almacenados existentes en la base de datos, por ejemplo:

- `SELECT * FROM public.listar_persona()`
- `CALL public.insertar_persona(...)`
- `CALL public.actualizar_persona(...)`
- `CALL public.eliminar_persona(...)`

Antes de probar el programa, la base de datos debe tener cargado el respaldo final con tablas, datos, funciones y procedimientos.

## Si aparece error de SSL con Supabase

Agrega esto a la cadena:

```text
SSL Mode=Require;Trust Server Certificate=true
```

## Si aparece error de procedimiento no existe

Verifica que el respaldo cargado en Supabase tenga las funciones y procedimientos con el mismo nombre que usa el programa.

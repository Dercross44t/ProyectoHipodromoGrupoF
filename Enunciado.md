# Encabezado del Proyecto

* [cite_start]**Institución:** Universidad de Costa Rica, Sede Liberia[cite: 4].
* [cite_start]**Carrera:** Informática Empresarial (SR-CIE)[cite: 2, 3].
* [cite_start]**Curso:** IF-5100 Administración Bases de Datos[cite: 5].
* **Profesor:** Msc. [cite_start]Andrés Alberto Cortés Fuentes (andres.cortesfuentes@ucr.ac.cr)[cite: 6].
* [cite_start]**Proyecto Final:** "Hipódromo Nacional - Sistema de Control de Carrera de Caballos"[cite: 7].
* [cite_start]**Valor:** 20%[cite: 8].

---

## Enunciado General

[cite_start]El Hipódromo Nacional es una organización dedicada a realizar carreras de caballos, controlar establos y gestionar de manera integral competencias ecuestres[cite: 10]. [cite_start]Por el crecimiento de sus operaciones, implementará un sistema de base de datos para gestionar eventos, caballos, propietarios, suministros, alimentación, controles veterinarios y facturación[cite: 11]. 

### Tipos de Usuarios

* [cite_start]**Propietarios:** Registran caballos, inscriben animales en eventos, y consultan resultados y facturación[cite: 13].
* [cite_start]**Administradores:** Gestionan eventos, controlan establos, validan inscripciones y supervisan la facturación[cite: 14].
* [cite_start]**Personal Veterinario:** Registra controles médicos y certificaciones sanitarias[cite: 15].
* [cite_start]**Encargados de Establo:** Gestionan alimentación, suministros y asignación de espacios[cite: 16].

### Flujo Principal del Sistema

1. [cite_start]El propietario registra su caballo[cite: 18].
2. [cite_start]El sistema valida la documentación y certificación veterinaria[cite: 19].
3. [cite_start]El caballo se asigna a un establo[cite: 20].
4. [cite_start]El administrador programa un evento de carrera[cite: 21].
5. [cite_start]El propietario inscribe su caballo en la competencia[cite: 22].
6. [cite_start]El sistema valida las condiciones médicas y reglamentarias[cite: 25].
7. [cite_start]Se realiza la carrera y se registran los resultados[cite: 26].
8. [cite_start]El sistema genera la facturación[cite: 27].
9. [cite_start]Se actualiza el historial y estadísticas del caballo y del propietario[cite: 28].

### Casos de Uso a Considerar

* [cite_start]Registro y autenticación de propietarios[cite: 30].
* [cite_start]Gestión de registro de caballos (raza, edad, peso, estado de salud)[cite: 33].
* [cite_start]Programación y gestión de eventos[cite: 34].
* [cite_start]Control de inscripción a carreras[cite: 35].
* [cite_start]Control de establos y asignación de caballos[cite: 36].
* [cite_start]Gestión de suministros y alimentación[cite: 37].
* [cite_start]Registro de historial veterinario[cite: 38].
* [cite_start]Generación de reportes de resultados y facturación[cite: 39].

---

## Descripción de la Base de Datos

* [cite_start]**Propietarios:** Identificación, nombre, apellidos, dirección completa (país, provincia, cantón, distrito, barrio), teléfonos y correos electrónicos (ambos deben permitir múltiples registros por tipo)[cite: 46].
* [cite_start]**Caballos:** Código único, nombre, fecha de nacimiento, sexo, raza, peso, estado de salud y propietario[cite: 47].
* [cite_start]**Eventos:** Código de evento, nombre, fecha, tipo de carrera, distancia, premio total y estado[cite: 48].
* [cite_start]**Inscripciones:** Código de inscripción, evento, caballo, fecha de inscripción y estado[cite: 49].
* [cite_start]**Establos:** Código, ubicación, capacidad y estado[cite: 50].
* [cite_start]**Suministros:** Código, tipo, proveedor, cantidad disponible y fecha de ingreso[cite: 51].
* [cite_start]**Alimentación:** Consumo por caballo, tipo de alimento, fecha y cantidad[cite: 52].
* [cite_start]**Historial Veterinario:** Código de registro, caballo, diagnóstico, tratamiento, fecha de revisión y veterinario[cite: 53].
* [cite_start]**Facturación:** Código, propietario, evento, subtotal, descuento, impuestos, total y estado de pago[cite: 54].
* [cite_start]**Historial de transacciones:** Registros detallados de pagos[cite: 55].

---

## Actividades del Proyecto

> [cite_start]**Nota importante:** El trabajo es en grupos definidos y el coordinador es el único representante ante el profesor[cite: 62]. [cite_start]Para llenar las tablas, se puede usar Mockaroo para generar un mínimo de 20 registros diferentes por tabla[cite: 63].

1. [cite_start]**Modelado:** Normalizar hasta la tercera forma normal (3FN) [cite: 65] [cite_start]y elaborar el modelo entidad-relación con tablas, campos, dependencias, llaves primarias y foráneas[cite: 66].
2. [cite_start]**Implementación:** Crear tablas, llaves [cite: 68][cite_start], restricciones de integridad [cite: 69] [cite_start]y tablas particionadas para las bitácoras[cite: 70].
3. [cite_start]**Formularios:** Desarrollar formularios de mantenimiento (ingreso, modificación y eliminación) [cite: 72] [cite_start]que invoquen procedimientos almacenados[cite: 73].
4. [cite_start]**Seguridad:** Implementar bitácoras en tablas particionadas por trimestre [cite: 75, 78][cite_start], triggers de auditoría [cite: 76] [cite_start]y capturar automáticamente usuario, acción y fecha en tiempo real[cite: 77].
5. [cite_start]**Automatización:** Crear procedimientos para validar certificación veterinaria [cite: 80][cite_start], calcular premios [cite: 81][cite_start], facturar según la legislación de Costa Rica [cite: 82] [cite_start]y otorgar beneficios a clientes frecuentes[cite: 83].

---

## Rúbrica de Evaluación

| Criterio | Valor | Descripción |
| :--- | :--- | :--- |
| **Infraestructura** | 5% | [cite_start]Implementar servidor de BD e interfaz gráfica generando valor público[cite: 90]. [cite_start]Penalización de -10% por un producto mediocre[cite: 91]. |
| **Normalización** | N/A | [cite_start]Normalizar a 3FN[cite: 92]. [cite_start]Manejar múltiples teléfonos/correos [cite: 93] [cite_start]y descomponer la dirección (País, Provincia, Cantón, Distrito, Barrio) con reglas de integridad[cite: 94]. |
| **Modelo E-R** | 2% | [cite_start]Elaborar modelo entidad-relación explícito con tablas, campos y llaves[cite: 95]. |
| **Creación BD** | N/A | [cite_start]Crear base de datos [cite: 96] [cite_start]y generar un Backup subido a Google Drive[cite: 97]. |
| **Formularios de Ingreso** | N/A | [cite_start]Formularios usando procedimientos almacenados (INSERT)[cite: 98]. |
| **Formularios de Eliminación**| N/A | [cite_start]Formularios usando procedimientos almacenados (DELETE)[cite: 99]. |
| **Formularios de Actualización**| N/A | [cite_start]Formularios usando procedimientos almacenados (UPDATE)[cite: 100]. [cite_start]Cada tabla requiere un procedimiento que actualice mínimo 3 valores y se debe justificar la elección[cite: 101]. |
| **Bitácoras Particionadas** | 3% | [cite_start]Crear bitácoras particionadas por trimestre para toda tabla con más de 5 campos[cite: 107]. [cite_start]Deben registrar usuario, acción y fecha en tiempo real[cite: 106]. |
| **Triggers de Auditoría** | N/A | [cite_start]Implementar triggers para llenar las bitácoras al ingresar, modificar o eliminar datos [cite: 108] [cite_start]en tablas con más de 5 campos[cite: 109]. |
| **Alerta Veterinaria** | 2% | [cite_start]Trigger que envíe una alerta al propietario al vencer la certificación veterinaria[cite: 110]. |
| **Descuento de Facturación** | 5% | [cite_start]Procedimiento que detecte si un propietario facturó más de 500 mil colones en los últimos 6 meses, actualice el campo de descuento para aplicar un 10% en la próxima factura [cite: 111, 112][cite_start], y lo reinicie después[cite: 113]. [cite_start]Validar en el videotutorial[cite: 114]. |
| **Módulo Facturación** | 3% | [cite_start]Objeto de BD para facturar eventos contemplando inscripción, subtotal, impuestos, etc. [cite: 115][cite_start], investigando la legislación costarricense[cite: 116]. |

---

## Entregables y Reglas de Entrega

**Documentos a entregar:**
1. [cite_start]Reporte del Coordinador (PDF)[cite: 122].
2. [cite_start]Videotutorial[cite: 122].
3. [cite_start]Manual Técnico de Implementación (PDF)[cite: 123].

**Instrucciones de subida:**
* [cite_start]Colocar todos los entregables en una carpeta compartida en Google Drive[cite: 124].
* [cite_start]El Coordinador debe subir un documento de texto a Mediación Virtual únicamente con la URL de la carpeta[cite: 124].
* [cite_start]La carpeta principal debe llamarse `Proyecto_Grupo_#`[cite: 128].
* [cite_start]Los archivos internos deben llamarse: `Reporte-Coordinador-Grupo-#` [cite: 129][cite_start], `Manual-Tecnico-Grupo_#` y `Videotutorial-Grupo_#`[cite: 130].

---

## Penalizaciones

* [cite_start]**Problemas de acceso:** La nota será de 0 si el docente no tiene acceso al recurso de Drive al momento de revisar[cite: 125].
* [cite_start]**Faltas de formato y nombres:** Cada falta a la estructura del documento "Estructura Documentos Escritos.pdf" o a las reglas de nombres de entrega se penaliza con -5% del porcentaje obtenido[cite: 132].
* [cite_start]**Falta de entregables:** La no presentación de alguno de los entregables se penaliza con -10% del porcentaje obtenido[cite: 133].
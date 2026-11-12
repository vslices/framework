# Prueba integral ciega — campaña de inscripción a actividades (iteración 2)

**Entregar este archivo al agente evaluado; NO entregar `expected.md` ni `activity-campaign-expected.md`.** El caso es deliberadamente acotado a Spaces; los comportamientos de búsqueda e inscripción se describen para establecer requisitos semánticos, no para solicitar una aplicación completa.

## Dominio

Se está construyendo un módulo para administrar una campaña de inscripción a actividades.

### Actividad

- Cada actividad tiene un identificador **artificial**, un nombre y capacidad.
- No se especificó el tipo subyacente, formato serializado ni política de generación del identificador artificial.
- El nombre es obligatorio. No se especificaron longitud, reglas de espacios ni caracteres permitidos.
- Puede existir una descripción. Si no se proporciona, significa que la actividad no tiene descripción; una descripción sin contenido significativo no es válida.
- La capacidad se establece al crear la actividad, debe ser un entero positivo y **no puede modificarse después**.
- El nombre y la descripción pueden cambiar una vez creada la actividad.
- La identidad de la actividad debe permanecer estable entre evoluciones.

### Participantes e inscripciones

- Cada participante inscrito tiene identificador propio, nombre y puede estar asociado a una cuenta administrada por **otro sistema**. El módulo actual no es propietario de esas cuentas.
- El identificador del participante es artificial; no se proporcionaron tipo subyacente ni formato.
- No se especificaron requisitos adicionales para el nombre del participante.
- Una persona inscrita puede no tener cuenta asociada. La representación de la identidad de cuenta está determinada por el sistema externo, cuyo contrato aún no está disponible.
- Una actividad tiene una colección ordenada de participantes inscritos. Al crearse, puede estar vacía.
- Dos participantes con el mismo identificador no pueden aparecer inscritos simultáneamente en la misma actividad.
- El orden de inscripción es significativo y debe preservarse.
- Después de la creación se pueden añadir participantes al final, sin superar la capacidad. Los ya inscritos no se eliminan ni se reordenan en este escenario.
- El identificador de un participante ya inscrito no puede sustituirse por otro durante la evolución de esa inscripción.
- No se definieron políticas adicionales sobre participantes asociados a una misma cuenta externa.

### Búsqueda por nombre

- El módulo necesita buscar actividades por prefijo de nombre, **sin distinguir mayúsculas de minúsculas**. Por ejemplo, una búsqueda por el prefijo `ac` debe encontrar tanto `ACT 1` como `ACt 2`.
- Debe conservarse la grafía original del nombre para presentarla.
- No se definió si la búsqueda ignora diacríticos, signos, espacios u otras equivalencias Unicode.
- En el futuro, persistencia podría optimizar la búsqueda mediante una columna derivada, pero ese mecanismo no forma parte del problema que se implementará ahora.

## Encargo

Usa la Skill `vslices-spaces-development` del repositorio `https://github.com/vslices/framework`, en `.agents/skills/vslices-spaces-development/SKILL.md`, y consulta sus referencias y los contratos actuales del Framework. **No consultes ninguna ruta bajo `tests/skills/vslices-spaces-development/` durante la resolución:** este texto se te entrega directamente como enunciado, y ese directorio contiene materiales privados de evaluación.

Modela e implementa los Spaces pertinentes para este dominio en C#, con su construcción, evolución cuando corresponda y pruebas representativas. Puedes elegir nombres de tipos y organización razonables acordes al Framework. Limita la implementación al modelo semántico: no construyas una base de datos, API ni Feature completa.

Entrega el código o archivos completos que hayas podido producir, indica qué compilaste y ejecutaste realmente, y termina con el informe de modelado exigido por la Skill. Si una decisión de dominio es desconocida, no la presentes como regla confirmada.

No consultes respuestas de evaluaciones previas ni infieras reglas únicamente de convenciones del lenguaje.

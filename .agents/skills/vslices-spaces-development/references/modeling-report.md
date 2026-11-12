# Informe obligatorio al finalizar el modelado de Spaces

Emitir un informe **incluso cuando no existan hallazgos**. El objetivo es hacer examinables las diferencias entre reglas explícitas, convenciones predeterminadas, lagunas y contradicciones. No confundir una especificación incompleta con un modelo incorrecto.

## Categorías

- **Incoherencia comprobada:** dos restricciones no pueden cumplirse simultáneamente; o el código permite un estado expresamente prohibido; o se representan como diferentes dos estados que el dominio declara equivalentes. Adjuntar regla o ejemplo que demuestre la contradicción.
- **Información no especificada:** falta una regla relevante, pero la ausencia no demuestra un error. No asumir que toda definición debe tener un máximo, mínimo, normalización o lista de caracteres permitidos. Documentar lo desconocido y su consecuencia.
- **Decisión provisional:** se usó una convención predeterminada para avanzar, sin atribuirla al dominio. Registrar el valor elegido y qué decisión futura puede reemplazarlo.
- **Riesgo o límite de verificación:** comportamiento aún no probado, contratos de integración no consultados, efectos de evaluación diferida, límites de persistencia desconocidos o prueba no ejecutada. Distinguir evidencia real de hipótesis.

## Preguntas de revisión pertinentes

1. **Identificador artificial:** ¿se especificó backing type y formato? Si no, utilizar `Guid` como backing type **provisional**, dentro de un Space nominal; reportar ambas ausencias. No decidir sin autoridad si se genera externamente, se acepta `Guid.Empty`, se serializa en un formato particular, ni cómo se almacena.
2. **Restricciones ausentes:** ¿se conocen admisibilidad, tamaño, rango, significado de vacío y reglas de normalización para los conceptos que las necesitan? Identificar las que falten sin inferir que necesariamente deben existir. Ejemplo: `ActivityName` sin límite conocido; el Space sigue siendo válido como concepto.
3. **Igualdad e identidad:** ¿está especificado si dos valores son iguales por identificador, contenido, normalización o contexto? ¿La regla de duplicados se basa en la igualdad correcta?
4. **Cardinalidad, orden y ausencia:** ¿se conoce si vacío es válido, si la colección puede no existir, si admite duplicados y si el orden importa? No usar `Option<Seq<A>>` sin una distinción entre inexistente y vacía.
5. **Transiciones:** ¿se sabe qué propiedades pueden cambiar, qué debe permanecer igual y cuáles son los estados de origen/destino admisibles? Si solo existen reglas de estado, no inventar operaciones ni restricciones de transición.
6. **Autoridad y fronteras:** ¿se pasan primitivas de .NET como conceptos dentro de un Space compuesto? ¿Se omite la validación de un Space propietario? ¿Se están creando métodos de Feature dentro del Space?
7. **Evidencia y límites técnicos:** ¿las invariantes de una colección diferida quedaron verificadas antes de aceptar el Space? ¿Se ejecutaron compilación, pruebas y Laws? ¿Hay supuestos de persistencia, tamaños o serialización no declarados?

## Formato mínimo

Presentar una tabla o apartados breves con: **Categoría | Concepto | Hallazgo y evidencia | Consecuencia | Decisión actual / pregunta pendiente**.

### Ejemplo (ficticio)

| Categoría | Concepto | Hallazgo y evidencia | Consecuencia | Decisión / pendiente |
| --- | --- | --- | --- | --- |
| Decisión provisional / información no especificada | `ParticipantId` | No se definió backing type ni formato artificial | Integración y representación podrían cambiar | Usar `Guid` provisionalmente; confirmar contrato |
| Información no especificada | `ActivityName` | No se indicó longitud máxima o política de espacios | El dominio todavía no delimita esos casos | Conservar `ActivityName`; no inventar máximo |
| Incoherencia comprobada | `Participants` | Regla prohíbe IDs duplicados, pero `Transform` los acepta | Estado inválido representable | Corregir transformación y probar |
| Riesgo de verificación | `Activity` | No se ejecutaron las pruebas de evolución | Admisibilidad de transición no demostrada | Registrar prueba pendiente |

Si no hay incoherencias comprobadas, declarar **«Sin incoherencias verificadas»**; pueden seguir existiendo vacíos o riesgos. Priorizar lo que modifica la semántica o el comportamiento; no fabricar hallazgos cosméticos. No agregar reglas solo para dejar el informe vacío.

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
3. **Igualdad e identidad:** ¿está especificado si dos valores son iguales por identificador, contenido, normalización o contexto? ¿La regla de duplicados se basa en la igualdad correcta? ¿Se está usando erróneamente la igualdad por identidad como prueba de que no cambió ningún dato relevante?
4. **Selecciones mantenidas por el dominio:** ¿el caso reconoce un conjunto completo de constantes semánticas? Si corresponde, ¿`MaintainedSpace<SELF>.All` incluye todos y solo los valores admitidos por la construcción? ¿Se está confundiendo una selección cerrada con catálogo administrable u opciones de interfaz? Consultar [MaintainedSpace](maintained-space.md).
5. **Alternativas y uniones discriminadas:** ¿el concepto admite alternativas mutuamente excluyentes con significados propios (p. ej., nombre de pila o razón social)? ¿La representación expresa exactamente una, sin permitir ninguna o ambas por accidente? ¿Se conocen el discriminador y las restricciones de cada variante? Consultar [uniones discriminadas](discriminated-unions.md).
6. **Cardinalidad, orden y ausencia:** ¿se conoce si vacío es válido, si la colección puede no existir, si admite duplicados y si el orden importa? No usar `Option<Seq<A>>` sin una distinción entre inexistente y vacía.
   **Valor ausente frente a valor vacío:** si el concepto admite ambos, ¿representan hechos distintos (p. ej., campo condicional no mostrado frente a mostrado sin contenido)? No eliminar esa distinción por analogía con otros dominios. ¿Existe evidencia para comprobar visibilidad en el request sin imputar esa responsabilidad a `Text`? ¿El contrato de transporte conserva las alternativas o sigue sin especificarse?
7. **Transiciones y Evolvable:** ¿se sabe qué propiedades pueden cambiar, qué debe permanecer igual y cuáles son los estados de origen/destino admisibles? ¿Los miembros inmutables de `State` son `get`-only y los modificables `init`? ¿`Evolve` valida continuidad e invariantes aun si `Update` devuelve un `State` ajeno o se invoca directamente la regla estática? ¿Cada invariante cuenta con autoridad propia, incluyendo relaciones entre estados? ¿Se rechazan evoluciones válidas por reglas inventadas? ¿Hay sobrecargas o construcciones públicas heredadas que eludan el contrato actual y las invariantes? Si solo existen reglas de estado, no inventar operaciones ni restricciones de transición. Consultar [evolvable.md](evolvable.md) cuando corresponda.
8. **Autoridad y fronteras:** ¿Un identificador pertenece al modelo actual o representa una entidad de otro sistema? Para vínculos externos, seguir la convención `<Concept>Reference` (p. ej., `AccountReference`) y registrar el contrato remoto no especificado. ¿Una normalización requerida se deriva como `NormalizedValue` dentro del Space, sin imponer un tipo adicional ni asumir reglas de colación? Ver [proyecciones y referencias externas](semantic-projections.md).
9. **Composición semántica:** ¿se pasan primitivas de .NET como conceptos dentro de un Space compuesto? ¿Se omite la validación de un Space propietario? ¿Se están creando métodos de Feature dentro del Space?
10. **Evidencia y límites técnicos:** ¿las invariantes de una colección diferida quedaron verificadas antes de aceptar el Space? ¿Se ejecutaron compilación, pruebas y Laws? ¿Hay supuestos de persistencia, tamaños o serialización no declarados?

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

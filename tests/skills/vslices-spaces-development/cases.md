# Casos ciegos — vslices-spaces-development

Presentar cada entrada a un agente SIN `expected.md`. Pedir significado, representación propuesta, distinciones de estado, invariantes y contraejemplos. No proporcionar respuestas esperadas durante la ejecución.

## E01 — Descripción opcional

«Un artículo puede tener una descripción. La cadena vacía no describe nada; no hay distinción entre descripción vacía y no definida».

## E02 — Cero órdenes

«Toda orden conserva sus líneas de pedido. Inicialmente no hay ninguna y eso es válido. Puede agregar líneas después; no tenemos otras reglas».

## E03 — Unicidad, vacío admitido

«La lista de líneas puede estar vacía, pero dos líneas nunca pueden compartir el mismo ItemId. El orden importa».

## E04 — No vacío

«Una orden confirmada debe tener al menos una línea, y el orden importa».

## E05 — Ausente frente a vacío

«Un catálogo puede no haber sido configurado. Eso difiere de haberlo configurado con cero productos: al configurarlo vacío, se deshabilitan las ventas».

## E06 — Posiciones opcionales

«Hay cuatro posiciones numeradas; cada posición puede estar libre u ocupada. Las cuatro posiciones siempre existen».

## E07 — Evitar abstraer demasiado

«El nombre es obligatorio y sus reglas de construcción ya las garantiza `ItemName`. No hay otras condiciones».

## E08 — Ambigüedad intencional

«El expediente puede tener documentos adjuntos».

## E09 — Diferir evaluación

«El conjunto de identificadores debe ser único, pero se entrega un `Iterable<Id>` asíncrono cuyo contenido aún no ha sido recorrido».

## E10 — Restricción semántica que no se deduce de Option

«El código de cupón puede faltar. Cuando existe, debe tener entre 6 y 12 caracteres. Un string vacío significa que no hay cupón».

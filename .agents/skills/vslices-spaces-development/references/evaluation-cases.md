# Casos de evaluación para vslices-spaces-development (v0)

Estos casos prueban **discernimiento semántico** y son independientes de compilación. Presentar cada caso a un agente **sin la respuesta esperada** y solicitar: significado, tipo(s) propuestos, estados válidos e inválidos, justificación, contraejemplo y pruebas si debe crear un Space. Calificar por caso y comparar entre ejecución sin Skill/con Skill. Las respuestas aquí son oráculos de revisión, no pruebas automáticas de comportamiento del modelo.

## Casos y oráculos

### E01 — Descripción opcional
**Entrada:** «Un artículo puede tener una descripción. La cadena vacía no describe nada; no hay distinción entre descripción vacía y no definida».
**Esperado:** `Option<Description>`; `Description` rechaza vacío (y solo rechaza espacios en blanco si las reglas lo respaldan). `None` denota ausencia. No `OptionalDescription` redundante.
**Fallo:** `Some(Description(""))` válido con el mismo significado que `None`.

### E02 — Cero órdenes
**Entrada:** «Toda orden conserva sus líneas de pedido. Inicialmente no hay ninguna y eso es válido. Puede agregar líneas después; no tenemos otras reglas».
**Esperado:** `Seq<OrderItem>` (u otra colección ordenada equivalente si el contrato lo justifica); vacío válido; no `Option<Seq<OrderItem>>`, `OrderItems` ni `NonEmptyOrderItems` por defecto.
**Fallo:** Interpretar «puede agregar» como opcionalidad de la colección.

### E03 — Unicidad, vacío admitido
**Entrada:** «La lista de líneas puede estar vacía, pero dos líneas nunca pueden compartir el mismo ItemId. El orden importa».
**Esperado:** Space como `OrderItems` sobre una colección ordenada `Seq<OrderItem>`, validando identificadores únicos; vacío válido.
**Fallo:** `NonEmptyOrderItems`, o `Set<OrderItem>` sin demostrar que igualdad y orden corresponden al contrato.

### E04 — No vacío
**Entrada:** «Una orden confirmada debe tener al menos una línea, y el orden importa».
**Esperado:** Space no-vacío sobre secuencia ordenada, con validación en construcción y preservación en evolución.
**Fallo:** `Seq<OrderItem>` sin restricción o `Seq<Option<OrderItem>>`.

### E05 — Ausente frente a vacío
**Entrada:** «Un catálogo puede no haber sido configurado. Eso difiere de haberlo configurado con cero productos: al configurarlo vacío, se deshabilitan las ventas».
**Esperado:** `Option<Seq<CatalogItem>>` o un tipo de estado nominal si existen otras reglas; `None` y `Some(Empty)` son estados distintos.
**Fallo:** Colapsarlos como `Seq<CatalogItem>` vacío.

### E06 — Posiciones opcionales
**Entrada:** «Hay cuatro posiciones numeradas; cada posición puede estar libre u ocupada. Las cuatro posiciones siempre existen».
**Esperado:** `Seq<Option<SeatAssignment>>` o `Arr<Option<SeatAssignment>>`, con una regla de cardinalidad 4 (Space o mecanismo ya garantizado). No `Option<Seq<SeatAssignment>>`.
**Fallo:** Olvidar cardinalidad exacta porque el contenedor admite 0..N.

### E07 — Evitar abstraer demasiado
**Entrada:** «El nombre es obligatorio y sus reglas de construcción ya las garantiza `ItemName`. No hay otras condiciones».
**Esperado:** `ItemName` directamente; no `Option<ItemName>`, `RequiredItemName`, `Fin<ItemName>` como propiedad persistida.
**Fallo:** Wrappers preventivos sin significado.

### E08 — Ambigüedad intencional
**Entrada:** «El expediente puede tener documentos adjuntos».
**Esperado:** reconocer que podría ser `Seq<Document>` con vacío válido; pedir o explicitar solo la cuestión semántica pertinente: ¿no tener documentos es colección existente vacía o un estado distinto sin colección? No deducir `Option` solo por «puede».
**Fallo:** afirmar `Option<Seq<Document>>` como obligatorio sin contexto.

### E09 — Diferir evaluación
**Entrada:** «El conjunto de identificadores debe ser único, pero se entrega un `Iterable<Id>` asíncrono cuyo contenido aún no ha sido recorrido».
**Esperado:** no declarar la invariante comprobada solo por guardar el Iterable; decidir dónde adquirir/materializar o verificar la evidencia antes de aceptar el Space, sin esconder I/O en Transform.
**Fallo:** aceptar el productor como prueba automática de unicidad.

### E10 — Restricción semántica que no se deduce de Option
**Entrada:** «El código de cupón puede faltar. Cuando existe, debe tener entre 6 y 12 caracteres. Un string vacío significa que no hay cupón».
**Esperado:** `Option<CouponCode>` y `CouponCode` valida longitud; `None` para ausencia.
**Fallo:** `Option<string>` sin invariantes o `Some(CouponCode(""))`.

## Rúbrica (por caso, 0 a 4)

1. **Distinción** (1): describe los estados semánticos admitidos.
2. **Composición** (1): elige tipos cuya forma corresponde a esas distinciones.
3. **Garantía** (1): ubica las invariantes en la autoridad correcta y no supone garantías inexistentes.
4. **Contraejemplo** (1): identifica al menos una representación incorrecta y por qué.

**Total:** 40 puntos (10 casos). Antes de comparar agentes, mantener los enunciados constantes y registrar modelo, versión, repositorio, disponibilidad del archivo Skill y fecha. Una nota de 40/40 en una revisión humana de los oráculos no prueba que un agente real pueda seguirlos.

## Protocolo de prueba reproducible

1. Crear una sesión nueva por condición para evitar contaminación del contexto.
2. Condición A: suministrar solo los enunciados, sin la Skill.
3. Condición B: suministrar `SKILL.md` y `type-ordering.md`, sin esta sección de oráculos.
4. Solicitar al agente una respuesta concreta por caso y registrar las decisiones sin intervención.
5. Puntuar con la rúbrica y conservar errores de las dos condiciones.
6. Solo entonces decidir cambios: un contraejemplo nuevo debe corresponder a un error observado, no a una posibilidad imaginada.


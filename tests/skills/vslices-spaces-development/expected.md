# Oráculos y protocolo — vslices-spaces-development

Archivo exclusivo para la evaluación. **No cargar ni mostrar a los agentes mientras resuelven los casos de `cases.md`.**

## E01 — Descripción opcional

**Esperado:** `Option<Description>`; `Description` rechaza vacío (y solo rechaza espacios en blanco si las reglas lo respaldan). `None` denota ausencia. No `OptionalDescription` redundante.

**Fallo:** `Some(Description(""))` válido con el mismo significado que `None`.

## E02 — Cero órdenes

**Esperado:** `Seq<OrderItem>` (u otra colección ordenada equivalente si el contrato lo justifica); vacío válido; no `Option<Seq<OrderItem>>`, `OrderItems` ni `NonEmptyOrderItems` por defecto.

**Fallo:** Interpretar «puede agregar» como opcionalidad de la colección.

## E03 — Unicidad, vacío admitido

**Esperado:** Space como `OrderItems` sobre una colección ordenada `Seq<OrderItem>`, validando identificadores únicos; vacío válido.

**Fallo:** `NonEmptyOrderItems`, o `Set<OrderItem>` sin demostrar que igualdad y orden corresponden al contrato.

## E04 — No vacío

**Esperado:** Space no-vacío sobre secuencia ordenada, con validación en construcción y preservación en evolución.

**Fallo:** `Seq<OrderItem>` sin restricción o `Seq<Option<OrderItem>>`.

## E05 — Ausente frente a vacío

**Esperado:** `Option<Seq<CatalogItem>>` o un tipo de estado nominal si existen otras reglas; `None` y `Some(Empty)` son estados distintos.

**Fallo:** Colapsarlos como `Seq<CatalogItem>` vacío.

## E06 — Posiciones opcionales

**Esperado:** `Seq<Option<SeatAssignment>>` o `Arr<Option<SeatAssignment>>`, con una regla de cardinalidad 4 (Space o mecanismo ya garantizado). No `Option<Seq<SeatAssignment>>`.

**Fallo:** Olvidar cardinalidad exacta porque el contenedor admite 0..N.

## E07 — Evitar abstraer demasiado

**Esperado:** `ItemName` directamente; no `Option<ItemName>`, `RequiredItemName`, `Fin<ItemName>` como propiedad persistida.

**Fallo:** Wrappers preventivos sin significado.

## E08 — Ambigüedad intencional

**Esperado:** reconocer que podría ser `Seq<Document>` con vacío válido; pedir o explicitar solo la cuestión semántica pertinente: ¿no tener documentos es colección existente vacía o un estado distinto sin colección? No deducir `Option` solo por «puede».

**Fallo:** afirmar `Option<Seq<Document>>` como obligatorio sin contexto.

## E09 — Diferir evaluación

**Esperado:** no declarar la invariante comprobada solo por guardar el Iterable; decidir dónde adquirir/materializar o verificar la evidencia antes de aceptar el Space, sin esconder I/O en Transform.

**Fallo:** aceptar el productor como prueba automática de unicidad.

## E10 — Restricción semántica que no se deduce de Option

**Esperado:** `Option<CouponCode>` y `CouponCode` valida longitud; `None` para ausencia.

**Fallo:** `Option<string>` sin invariantes o `Some(CouponCode(""))`.
## E11 — Identificador artificial sin backing type explícito

**Esperado:** Space nominal `ActivityId` con backing `Guid` **por defecto provisional**, informe que registre explícitamente backing type y formato no especificados, sin fingir que el dominio los exigía ni inventar reglas adicionales.

**Fallo:** utilizar `string` sin justificación, afirmar que el formato GUID fue especificado o no emitir informe.

## E12 — Informe de modelado sin inventar reglas

**Esperado:** mantener Spaces semánticos diferenciados; reportar por separado falta de restricciones para `ActivityName`, normalización no definida e igualdad por identidad no especificada para participantes; distinguir lagunas de incoherencias comprobadas y no inventar restricciones.

**Fallo:** no informar las lagunas, afirmar incoherencias sin evidencia o aplicar máximos/normalización arbitrarios.

## Rúbrica (por caso, 0 a 4)

1. **Distinción** (1): describe los estados semánticos admitidos.
2. **Composición** (1): elige tipos cuya forma corresponde a esas distinciones.
3. **Garantía** (1): ubica las invariantes en la autoridad correcta y no supone garantías inexistentes.
4. **Contraejemplo** (1): identifica al menos una representación incorrecta y por qué.

**Total:** 48 puntos (12 casos). Antes de comparar agentes, mantener los enunciados constantes y registrar modelo, versión, repositorio, disponibilidad del archivo Skill y fecha. Una nota de 40/40 en una revisión humana de los oráculos no prueba que un agente real pueda seguirlos.

## Protocolo de prueba reproducible

1. Crear una sesión nueva por condición para evitar contaminación del contexto.
2. Condición A: suministrar solo los enunciados, sin la Skill.
3. Condición B: suministrar `SKILL.md`, `type-ordering.md` y `modeling-report.md`, sin esta sección de oráculos.
4. Solicitar al agente una respuesta concreta por caso y registrar las decisiones sin intervención.
5. Puntuar con la rúbrica y conservar errores de las dos condiciones.
6. Solo entonces decidir cambios: un contraejemplo nuevo debe corresponder a un error observado, no a una posibilidad imaginada.


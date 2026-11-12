# Composición semántica de Spaces y tipos funcionales

## Regla principal

**Modelar distinciones de significado antes de elegir el orden de los tipos.** `Option<Space>`, `Seq<Space>`, `Space` que encapsula `Seq<A>` y `Option<Space<Seq<A>>>` no son intercambiables. Tampoco debe decidirse su forma solo por palabras como «puede», «tiene» o «nunca».

1. **Identificar el concepto** y sus reglas de admisibilidad propias.
2. **Identificar qué condiciona la regla**: la existencia del concepto, cada elemento, la cardinalidad o la relación entre elementos de una colección.
3. **Para valores individuales**, preguntar si la ausencia del valor es válida; si lo es, considerar `Option<Description>`. La ausencia y un valor construido no deben significar lo mismo. Si una descripción vacía carece de significado, `Description.Transform(string.Empty)` debe rechazarse; `None` ya expresa ausencia.
4. **Solo para estructuras con estado vacío**, preguntar si «colección inexistente» y «colección existente pero vacía» son situaciones semánticamente diferentes. Si ambas significan «aún no hay elementos», preferir `Seq<A>` vacío; no inventar `Option<Seq<A>>`. Esta pregunta no es universal para todo `A`.
5. **Si la estructura satisface el contrato sin reglas nuevas**, usar el tipo de LanguageExt directamente: `Seq<OrderItem>` (o `Arr`, `Lst` u otro cuando corresponda). Si una regla propia de la colección necesita autoridad y construcción, definir un Space que la envuelva: `OrderItems` sobre `Seq<OrderItem>` para prohibir identificadores duplicados, o `NonEmptyOrderItems` para exigir al menos uno.
6. **Solo si coexisten distinciones independientes**, componer capas: `Option<NonEmptyOrderItems>` es justificable únicamente cuando «el grupo no existe» significa algo diferente de «el grupo existe y contiene cero elementos». No deducirlo de «puede tener órdenes».
7. **Revisar los estados representables**. Dos representaciones distintas con exactamente el mismo significado son evidencia de redundancia; no prohibir estados distintos que sí tengan significado propio.
8. **Verificar garantías reales**: el envoltorio nominal no prueba una invariante por sí solo. Validar en la construcción y preservar en la evolución; tener cuidado con colecciones evaluadas de forma diferida.

## Ejemplos y contraejemplos

### Descripción que puede faltar

Regla: «Puede tener una descripción; cuando la tiene, debe haber contenido significativo».

```csharp
Option<Description> Description
```

`Description` establece sus invariantes (`string.Empty` no es una descripción admisible **en este dominio**). `None` significa ausencia de descripción.

**Incorrecto:** aceptar `Some(Description(""))` como equivalente de `None`, o crear `OptionalDescription` sin otro significado. Tampoco universalizar «ningún Space de texto acepta vacío»: otros conceptos pueden admitirlo con un significado diferente.

### Elementos que pueden ser cero o más

Regla: «La orden tiene elementos; ninguno ha sido agregado todavía» significa colección existente vacía.

```csharp
Seq<OrderItem> Items
```

**Incorrecto:** `Option<Seq<OrderItem>>` solo para representar que aún no hay elementos, porque `None` y `Some(Empty)` duplican el mismo estado.

### Colección que prohíbe duplicados

Regla: «Los elementos de la orden pueden ser cero o más, pero no puede haber dos con el mismo identificador».

```text
OrderItems
  owns Seq<OrderItem>
  Transform: aceptar vacío; rechazar IDs duplicados
```

**Incorrecto:** `NonEmptyOrderItems` (agrega una restricción no solicitada), o `Seq<OrderItem>` sin validación de unicidad. Considerar `Set<A>` solo si expresa fielmente la igualdad y el orden del dominio; no asumir que elimina duplicados por ID automáticamente.

### Colección obligatoriamente no vacía

Regla: «Una orden confirmada debe tener al menos un elemento».

```text
NonEmptyOrderItems
  owns Seq<OrderItem>
  Transform: rechazar vacío
```

**Incorrecto:** `Seq<OrderItem>` sin garantía adicional; `Seq<Option<OrderItem>>` modifica la presencia **de cada elemento**, no la cardinalidad de la colección.

### Colección opcional: caso excepcional justificado

Regla: «Un catálogo todavía no está configurado» es distinto de «ya fue configurado explícitamente con cero elementos».

```csharp
Option<Seq<CatalogItem>> Catalog
```

`None` = sin configuración; `Some(Empty)` = configuración completada y vacía. Ambas situaciones existen y provocan decisiones diferentes.

**Incorrecto:** reemplazarlo automáticamente por `Seq<CatalogItem>`, que perdería esa distinción. También sería incorrecto aplicar este patrón por analogía a cualquier colección.

### Opcionalidad por posición

Regla: «Hay posiciones numeradas; cada una puede encontrarse libre u ocupada».

```csharp
Seq<Option<SeatAssignment>> Seats
```

**Incorrecto:** `Option<Seq<SeatAssignment>>`, que modifica la existencia de toda la secuencia y pierde las posiciones vacantes.

### Condiciones combinadas

- `Seq<PositiveQuantity>`: cada elemento es positivo; colección vacía permitida.
- `NonEmptyQuantities` sobre `Seq<PositiveQuantity>`: elementos positivos y al menos uno.
- `Option<NonEmptyQuantities>`: además, el **grupo mismo puede no existir**, cuando esa distinción tenga significado comprobable.

## Colecciones y evaluación diferida

`Seq<A>` puede construirse desde una fuente diferida; `Iterable<A>` también puede diferir evaluación y admitir asincronía. No asumir que guardar el productor equivale a validar la invariante. Si el Space promete no-vacío, unicidad u otra regla global, comprobarla al construir, o disponer de garantías suficientes explícitas; no esconder I/O en una transformación pura.

## Lista de comprobación del agente

- ¿Qué concepto existe y qué significa?
- ¿La condición se refiere a la existencia del concepto, a cada elemento, o a la colección completa?
- Si hay colección vacía: ¿qué diferencia observable hay entre «vacía» e «inexistente»? **No preguntar esto para cada valor individual.**
- ¿Qué estados distintos admite el tipo propuesto? ¿Alguno representa exactamente lo mismo que otro?
- ¿Qué invariantes aporta el Space por encima del constructor funcional?
- ¿Qué ejemplo refuta la alternativa descartada?
- ¿Qué evidencia verifica admisibilidad y preservación de invariantes?

Esta referencia describe decisiones de modelado; no sustituye las firmas reales de LanguageExt ni `SpaceModelingLaws`.

# Orden de composición: Space y constructores de LanguageExt

La elección entre `F<Space>` y `Space<F<A>>` depende de **qué expresa la condición** y **quién establece su admisibilidad**. No inferir la forma por la redacción superficial de una frase: comprobar su significado.

## Regla de decisión

1. Identificar el concepto (`A` o un Space nominal) y las condiciones que definen que un ejemplar suyo es válido.
2. Distinguir condiciones **sobre presencia o multiplicidad del concepto ya válido** de condiciones **sobre los valores admitidos de una estructura completa**.
3. Si una forma estándar de LanguageExt expresa íntegramente la condición, componerla alrededor del concepto: `Option<Description>`, `Seq<OrderItem>`, `Arr<Part>`. Evitar crear un Space sin significado adicional.
4. Si la estructura completa está sometida a una regla semántica adicional (cardinalidad, unicidad, orden significativo, correlación entre elementos, etc.), crear un Space propietario de esa regla alrededor de la estructura: `NonEmptyOrderItems` que encapsula `Seq<OrderItem>`.
5. Aplicar las dos capas cuando coexistan condiciones distintas. Por ejemplo, `Option<NonEmptyOrderItems>` significa que **puede no existir** la colección, pero **si existe** no es vacía. No colapsar ambas condiciones en una sola.
6. Verificar la construcción y todas las operaciones que podrían exponer un estado inválido. El wrapper no basta: debe establecer y conservar efectivamente su invariante.

## Ejemplos y contraejemplos

### Descripción opcional — `Option<Description>`

Enunciado: «Puede tener una descripción». `Description` es un Space válido por derecho propio; el condicionante afecta **si está presente**.

```csharp
public readonly record struct Input(
    ItemName Name,
    Option<Description> Description);
```

**Contraejemplo:** crear `OptionalDescription` que encapsula `Option<Description>` **solo para reiterar** que puede estar presente o ausente. Sería estructura redundante. Podría justificarse un Space nominal si aporta significado o reglas propios y verificables (no por la mera opcionalidad).

### Órdenes sin restricción adicional — `Seq<OrderItem>`

Enunciado: «Tiene órdenes»; la colección puede ser vacía, respeta el orden y admite repeticiones si el dominio no dispone otra cosa.

```csharp
public readonly record struct Input(Seq<OrderItem> Items);
```

**Contraejemplo:** introducir `OrderItems` con un `Transform` que simplemente acepta cualquier `Seq<OrderItem>`, sin identidad, distinción, invariante o comportamiento adicional. No requiere wrapper nominal por defecto. Si «tiene órdenes» realmente implica al menos una, **sí** existe una condición adicional: confirmarla, no inferirla en silencio.

### Órdenes obligatoriamente no vacías — `NonEmptyOrderItems` sobre `Seq<OrderItem>`

Enunciado: «Tiene órdenes y nunca menos de una». La restricción afecta **a la colección completa**, no a cada `OrderItem` ni a la presencia de una colección opcional.

```csharp
// Esquema semántico, no implementación completa ni código compilado.
Space NonEmptyOrderItems
    owns Seq<OrderItem> Items
    Transform(seq) = Fail if seq is empty; otherwise accepted Space
```

**Contraejemplos:**
- `Seq<OrderItem>` directamente **no** garantiza no-vacío.
- `Option<Seq<OrderItem>>` admite `None` y `Some(empty)`; no garantiza no-vacío y añade ausencia.
- `Seq<Option<OrderItem>>` condiciona **cada elemento**, admite una secuencia vacía e incluso elementos `None`. No expresa la regla de cardinalidad.
- `NonEmptyOrderItem` aplicado a cada elemento restringe los elementos individualmente, no asegura que exista alguno.

### Dos condiciones independientes — `Option<NonEmptyOrderItems>`

Enunciado: «Puede existir un grupo de órdenes, y, cuando existe, debe contener una o más».

```csharp
public readonly record struct Input(
    Option<NonEmptyOrderItems> Items);
```

**Contraejemplo:** `NonEmptyOrderItems` sin `Option` haría la presencia obligatoria; `Option<Seq<OrderItem>>` admitiría `Some(empty)`. Ambas perderían una condición.

### Opcionalidad de cada elemento — `Seq<Option<A>>`

Enunciado: «Hay posiciones ordenadas; cada una puede estar ocupada o vacía». Las posiciones existen y su contenido es opcional.

```csharp
Seq<Option<SeatAssignment>> Seats
```

**Contraejemplo:** `Option<Seq<SeatAssignment>>` haría opcional **la secuencia completa**; no representaría vacíos por posición.

### Restricción sobre el elemento versus restricción sobre la colección

- `Seq<PositiveQuantity>`: **cada cantidad** debe ser positiva; la secuencia puede estar vacía.
- `NonEmptyQuantities` sobre `Seq<PositiveQuantity>`: cada cantidad es positiva **y** la colección no está vacía.
- `Option<NonEmptyQuantities>`: toda esa colección puede estar ausente.

Si el dominio exige unicidad, orden sin repeticiones u otra propiedad, considerar si `Set<A>` u otro tipo ya expresa la regla antes de escribir un wrapper sobre `Seq<A>`. No agregar tipos solo porque puedan ser imaginados.

## Atención a la evaluación diferida

`Seq<A>` puede construirse desde una fuente diferida; `Iterable<A>` puede ser diferido o asíncrono. Un Space que promete `NonEmpty`, unicidad u otras invariantes de contenido debe **establecer la garantía durante su construcción**, no limitarse a guardar el productor. Materializar o disponer de evidencia explícita y suficiente según el caso; no esconder I/O en una transformación pura. La comprobación de `IsEmpty` por sí sola no demuestra automáticamente todas las propiedades de una secuencia diferida.

## Comprobación del agente

Para cada tipo propuesto, explicitar:
- **Objeto**: ¿qué concepto se está modelando?
- **Condición**: ¿afecta al elemento, a su presencia, a cada posición o a la estructura completa?
- **Precedencia**: ¿por qué `Option<Space>`, `Seq<Space>` o `Space<Seq<A>>`?
- **Garantía**: ¿dónde se establece y quién conserva la invariante?
- **Contraejemplo**: ¿qué alternativa aparentemente similar admite valores que la regla prohíbe?

Este documento aporta criterios de modelado, no redefine las APIs reales de LanguageExt ni sustituye las `SpaceModelingLaws`.

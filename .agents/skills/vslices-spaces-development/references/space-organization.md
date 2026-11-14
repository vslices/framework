# Organización y características semánticas de Spaces

## Distribución de archivos

La convención predeterminada es **un Space concreto por archivo**, con nombre correspondiente al concepto. Evitar agregados extensos de definiciones independientes (por ejemplo, `EvaluationValues.cs`). No convertir la convención en una regla de runtime: los tests de reflexión no conocen archivos fuente.

Excepción: conservar juntas declaraciones que formen una **unidad semántica inseparable**, como la abstracción y variantes de una unión discriminada nominal. Evaluar la cohesión por significado, no por ahorrar archivos.

## Funciones puras como características

Una característica de un Space puede ser una función pura que expresa una regla del dominio. Por ejemplo:

```csharp
Func<TicketFlowFacts, Error> Eligibility
```

Una transición de Flow puede conservar esa función y evaluarla con hechos ya obtenidos. `Error.IsEmpty` significa que no hay impedimentos y un error compuesto puede explicar múltiples condiciones de inelegibilidad.

Distinguir responsabilidades:

- **Space:** define las reglas de elegibilidad y las propiedades semánticas de la transición.
- **Feature/Work:** consigue evidencia, decide ejecutar la acción y orquesta el avance.
- **Grounding:** realiza las operaciones externas necesarias.

La presencia de una función dentro del Space no justifica introducir `IO`, `Eff`, repositorios o llamadas de red en su implementación, ni implementar un motor de reglas, DSL o infraestructura especializada sin evidencia de necesidad.

## C# y dependencias actuales

Preferir nullability expresiva fuera de contratos semánticos que exijan opcionalidad algebraica explícita; respetar SM007 en `Input`/`State`. Usar pattern matching, expresiones modernas y composición legible. Las expresiones de colección `[]` para `Seq<T>` solo son recomendables tras verificar compatibilidad de la versión instalada de LanguageExt v5. No extrapolar limitaciones de LanguageExt v4.

Comprobar el contrato real de cada trait, la compilación y las pruebas de comportamiento antes de declarar un patrón válido.

## Namespaces canónicos de las definiciones del Framework

Dentro de `src/VSlices.Spaces/Finance/`, `Quantities/` (incluidas `Algebra/` y `Derived/`) y `Temporal/`, todas las definiciones viven en `VSlices.Spaces.Definitions`, independientemente de la subcarpeta. **Única excepción:** `src/VSlices.Spaces/Quantities/Abstract/` utiliza `VSlices.Spaces.Definitions.Abstract`. No inferir el namespace a partir de `Finance`, `Quantities` ni `Temporal`: las carpetas organizan los archivos, mientras que el contrato público usa la superficie de definiciones. En consumidores importar el namespace canónico y revisar referencias cualificadas durante la migración; esto cambia nombres CLR públicos.

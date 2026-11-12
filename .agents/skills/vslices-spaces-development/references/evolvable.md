# Evolvable: contrato, autoridad y garantías

Consultar primero el contrato vigente en `src/VSlices.Spaces/Traits/Evolvable.cs`. Esta referencia explica la realización actual; no presupone cambios de API.

## Cuándo usarlo

Usar `Evolvable<SELF, SELF.State>` cuando el Space represente un punto con estado aceptado que puede evolucionar, sin mutar la instancia anterior. No usarlo automáticamente en todo Space ni inventar un ciclo de vida a falta de reglas. El modelo puede estar incompleto: reportar qué transiciones y propiedades modificables no fueron especificadas.

## Separar tres responsabilidades

- **Feature:** decide la acción y construye una propuesta de estado (por ejemplo, inscripción de un participante).
- **`Update(Func<STATE,STATE>)`:** mecanismo de evolución de la instancia: obtiene `CurrentState`, aplica la función y pasa `old` y el resultado a la regla. La función puede devolver cualquier instancia de `STATE`.
- **`static Evolve(old, proposed)`:** regla propiedad del Space: decide si ese par de estados constituye una transición admisible, entrega `Fin<SELF>` y, si acepta, materializa un nuevo punto sin mutar el anterior. No es el caso de uso ni determina qué acción debe ejecutar la Feature.

No poner operaciones como `Enroll`, `Approve` o `Cancel` dentro del Space por defecto. Un ejemplo autocontenido puede condensar responsabilidades, pero no establecer esa forma como convención.

## State y mutabilidad selectiva

En la realización vigente:

- Anidar `State` dentro de `SELF` con constructor privado (SM003/SM004).
- Exponer `public State CurrentState { get; }` sin setter (SM005).
- Mantener la construcción de estados aceptados bajo la autoridad del Space; verificar la firma exacta si se usa `UnsafeAccessor` (mecanismo, no garantía).
- Para propiedades **no modificables en propuestas por `with`**, usar `public T Id { get; }`, **sin `init`**.
- Para propiedades cuya modificación **puede proponerse** por `with`, usar `public T Detail { get; init; }`. Un `init` no afirma que todo cambio sea semánticamente válido; `Evolve` decide.
- Preservar la validez interna de valores compuestos mediante sus Spaces propietarios, también si aparecen en una propuesta de evolución. Evitar estructuras internamente mutables que permitan eludir estas garantías.

```csharp
// Esquema ilustrativo de State, no implementación autónoma del trait.
public sealed record State
{
    private State(ActivityId id, ActivityName name, Participants participants)
    {
        Id = id;
        Name = name;
        Participants = participants;
    }

    public ActivityId Id { get; }
    public ActivityName Name { get; }
    public Participants Participants { get; init; }
}
```

Una Feature puede proponer `old with { Participants = acceptedParticipants }`, pero `old with { Id = otherId }` es rechazado por el compilador. Eso **solo** limita las copias con `with`.

## Límite: procedencia y continuidad entre estados

La firma actual `Update(Func<STATE,STATE>)` no demuestra que el resultado derive del argumento. Por ejemplo, `activityA.Update(_ => activityB.CurrentState)` es expresable. Además, `Activity.Evolve(activityA.CurrentState, activityB.CurrentState)` puede invocarse directamente.

Por tanto:

1. Conservar en `Evolve` las comprobaciones de identidad y demás invariantes que deban mantenerse ante **cualquier par** `old, proposed`, aun cuando `Id` tenga solo `get`.
2. No duplicar por rutina cualquier validación: distinguir qué garantiza el tipo (impide cierta asignación mediante `with`) de lo que solo puede verificarse al comparar estados arbitrarios.
3. No prometer prueba de procedencia, continuidad ni inmutabilidad profunda mediante `get`, `init`, `record` o `UnsafeAccessor`. En particular, un miembro `get` que referencia un objeto mutable no lo vuelve profundamente inmutable.
4. No cambiar la interfaz `Evolvable` ni inventar wrappers de propuestas solo para solucionar un problema hipotético: conservar el mecanismo actual y validar las reglas necesarias.

## Matriz mínima de pruebas

- `Transform` acepta estados iniciales válidos y rechaza los inválidos.
- `Update` con propuesta admisible produce un nuevo `SELF` y no altera al original.
- `Evolve` rechaza propuestas que cambien la identidad, incluso si ambos `State` fueron aceptados por construcciones independientes.
- `Evolve` rechaza transiciones inválidas que conservan la identidad: cardinalidad, unicidad, restricciones entre propiedades, orden o ciclo de vida **solo si el dominio las estableció**.
- Las validaciones de Spaces componentes se preservan durante la evolución.
- Comprobar mediante compilación o prueba de compilación negativa que `with { Id = ... }` no compila cuando `Id` es `get`-only; las Laws estructurales no prueban ese requisito ni todas las transiciones.
- Si no se puede compilar o no hay reglas suficientes, informar el límite en el informe obligatorio de modelado; no sustituir evidencia por afirmaciones.

## Preguntas para el informe

Registrar como **información no especificada** las propiedades que pueden evolucionar, las invariantes entre estados, la identidad estable, o la política de transición que sea relevante pero no esté definida. Registrar como **incoherencia comprobada** una ruta que acepte estados expresamente prohibidos. Registrar como **riesgo de verificación** las rutas de evolución y compilación que no fueron probadas. No convertir ausencia de datos en reglas inventadas.

---
name: vslices-spaces-development
description: >-
  Modelar, implementar y revisar definiciones semánticas de Spaces en VSlices Framework (.NET).
  Usar cuando una tarea cree o modifique valores, identidades, transformaciones, invariantes
  o estados evolvables en un módulo Spaces, antes de implementar Features que los consuman.
---

# Desarrollo de Spaces

## Propósito y alcance

Convertir **material conceptual ya disponible** (concepto, reglas, evidencia, invariantes y transiciones) en definiciones semánticas de Spaces. Si el material es insuficiente, explicitar las preguntas o supuestos pendientes: no inventar reglas de dominio. El modelado conceptual completo pertenece a `vslices-domain-modeling` cuando exista.

No implementar operaciones de Work, adquisición de datos, persistencia ni transporte dentro de un Space.

Los tipos funcionales de LanguageExt son parte de la base conceptual escogida por VSlices. **Esta Skill debe enseñar su papel en Spaces**, no delegar su selección semántica a otra guía. Consultar `language-ext-development` para composición, sobrecargas y API efectiva; el futuro fork de LanguageExt aún no es una dependencia implementada.

## Procedimiento mínimo

1. **Localizar la autoridad.** Identificar el repositorio y la versión efectiva de `VSlices.Spaces`; leer el código y los contratos instalados, no confiar en ejemplos desactualizados. Consultar [Modeling semantic Spaces](https://github.com/vslices/framework/blob/master/docs/spaces/modeling.md) y los traits concretos.
2. **Delimitar el significado.** Determinar qué representa la definición, qué identidad o igualdad le corresponde, qué evidencia necesita para existir y cuáles son sus invariantes. Distinguir datos externos de valores semánticos aceptados. Usar primitivas .NET como materia prima de Spaces básicos, no como sustitutos de conceptos identificados en Spaces compuestos. Un Space nominal puede existir aunque falten reglas: registrar las condiciones desconocidas sin inventarlas. Para identificadores artificiales, identificar el backing type y formato especificados. Si no se especifican, usar **`Guid` por defecto** como backing type del Space básico, señalar **explícitamente** que fue una decisión provisional y registrar que falta confirmar el contrato. No imponer un esquema de cadena, política de generación ni restricciones adicionales no suministradas. Distinguir identificadores naturales cuyo formato proviene del dominio.
3. **Determinar distinciones, precedencia y construcción.** Antes de elegir un constructor, identificar qué estados reconoce el dominio y cuáles serían redundantes. Consultar [composición semántica](references/type-ordering.md) y sus contraejemplos: decidir primero si la condición modifica la existencia del concepto, cada elemento o la colección completa; comparar ausencia con vacío **solo para estructuras donde el vacío sea representable**. Luego, para definiciones en un namespace `Definitions` sujeto a las Laws, declarar `SELF.Input` y realizar `Transformable<SELF.Input, SELF>` con `Fin<SELF> Transform(Input)`. Encapsular constructores. Aplicar la selección de tipos funcionales de la sección siguiente: requerir `A` cuando sea obligatorio; usar `Option<A>` solo cuando `None` sea un estado de información admisible en la construcción. No utilizar nullabilidad C# en `Input`/`State` sujetos a SM007.
4. **Decidir si hay evolución.** No aplicar `Evolvable` a toda definición. Si hay estado aceptado que puede cambiar, considerar `Evolvable<SELF, SELF.State>`, `CurrentState` de solo lectura y `Evolve(old, proposed)` validando la transición sin mutar la instancia original. **La Feature determina qué cambio se intenta; el Space determina si el cambio propuesto es válido.** No añadir por norma general métodos de operación como `Enroll`, `Approve` o `Cancel` al Space: esas acciones pertenecen a Features. En el `State` anidado, declarar **`{ get; }` sin `init`** para propiedades cuya modificación no se permite y **`{ get; init; }`** solo para aquellas que pueden participar en una propuesta mediante `with`. Preferir esta restricción comprobada por el compilador, pero no asumir que prueba la procedencia del nuevo estado: `Update(Func<State,State>)` puede recibir un `State` ajeno y `Evolve(old, proposed)` sigue siendo invocable directamente. Conservar en `Evolve` las comprobaciones necesarias para proteger identidad, continuidad e invariantes ante propuestas arbitrarias. Seguir la convención actual de `State` anidado y constructor privado cuando corresponda.
5. **Realizar lo mínimo.** Escribir la transformación pura con la evidencia explícita disponible. No introducir `IO`, consultas, servicios o adaptadores en `Transform` para obtener hechos faltantes. No confundir `UnsafeAccessor` (mecanismo de .NET para acceder a un constructor privado) con una garantía semántica.
6. **Probar y revisar.** Compilar. Ejecutar `SpaceModelingLaws.InspectDefinitions(assembly, definitionsNamespace)` o `Inspect(type)` en pruebas del consumidor y revisar los diagnósticos SM001–SM007. Agregar pruebas de comportamiento para aceptación/rechazo, igualdad, preservación de invariantes y transiciones admitidas/rechazadas. Las Laws son estructurales: **no prueban corrección semántica**. Declarar explícitamente lo no ejecutado o no demostrado.
7. **Emitir informe de modelado obligatorio.** Al finalizar, presentar las incoherencias verificadas, vacíos de especificación, decisiones predeterminadas y riesgos de validación, con la evidencia y la consecuencia de cada uno. Usar [criterios y formato del informe](references/modeling-report.md); si no hay hallazgos, indicarlo explícitamente, sin inventarlos.

## Identificadores, primitivas y responsabilidad del cambio

- Un identificador **artificial** requiere una representación y un contrato de formato. Consultar primero la decisión explícita del dominio o sistema. En ausencia de especificación, construir el Space básico con backing type **`Guid` por defecto** y reportar «backing type/formato no especificado; se utilizó Guid provisionalmente». No afirmar que el dominio exige GUID ni asumir sin evidencia políticas de generación, versionado, serialización o persistencia; no inventar restricciones más fuertes.
- Un identificador **natural** deriva sus restricciones del dominio que lo define; no imponerle por analogía un formato artificial.
- El tipo básico puede no tener todavía reglas adicionales conocidas. Eso no lo vuelve superfluo: `ActivityName` y `ParticipantId` pueden tener significado propio aunque su representación técnica sea `string`. No introducir valores primitivos como `string Name` en un Space compuesto donde se conoce el concepto `ActivityName`; convertirlos en la frontera de construcción del Space básico.
- **No confundir admisibilidad con acción.** `Evolve(old, proposed)` establece si una transición propuesta es admisible. La decisión de inscribir un participante y preparar la propuesta pertenece a una Feature; `Activity.Enroll(...)` no es la forma normal de expresar el caso de uso. Si un ejemplo didáctico condensa ambas responsabilidades, identificarlo como excepción de alcance, no como patrón.
- Probar transiciones admitidas y rechazadas por el Space, y probar por separado el comportamiento de la Feature que propone el cambio. No concluir que existe una Feature porque una clase Space tenga un método operativo.

## Composición semántica de los tipos

La precedencia se deriva del significado, no de palabras como «puede»:

- **Valor obligatorio**: `Description` cuando siempre debe haber una descripción válida.
- **Valor opcional**: `Option<Description>` cuando la ausencia es válida y diferente de una descripción construida. En ese dominio, `Description` debe rechazar cadenas vacías si solo significan ausencia.
- **Colección cero a N sin reglas adicionales**: `Seq<OrderItem>`; vacía puede significar «todavía no se agregaron elementos».
- **Colección con invariantes propias**: `OrderItems` sobre `Seq<OrderItem>` si, por ejemplo, prohíbe duplicados y **sigue admitiendo vacío**; `NonEmptyOrderItems` cuando prohíbe vacío.
- **Opcionalidad de la colección**: `Option<Seq<A>>` solo si «colección inexistente» difiere semánticamente de «colección existente vacía». No confundir con `Seq<Option<A>>`, que condiciona cada posición.

**Evitar estados semánticamente redundantes**: comprobar que cada alternativa representable aporta una distinción real. Leer [ejemplos y contraejemplos](references/type-ordering.md), y aplicar sus preguntas al analizar una nueva definición. No imponer los nombres o restricciones de los ejemplos a dominios distintos.

## Tipos de LanguageExt dentro de Spaces

| Tipo | Papel en el modelo semántico | Límite |
| --- | --- | --- |
| `A` | Valor obligatorio de `Input` o `State` | No envolverlo en `Option<A>` por anticipar fallos o datos futuros. |
| `Option<A>` | Ausencia válida **en el contrato semántico**; `Some`/`None` tienen significado definido. | No utilizarlo si el propio `A` admite un estado que solo significa ausencia: revisar primero sus invariantes. |
| `Seq<A>`, `Arr<A>`, `Lst<A>` | Colecciones inmutables de cero a N valores; elegir por estructura y operaciones requeridas. | No garantizan por sí solas no-vacío, unicidad según identidad de dominio u otras invariantes globales. |
| `Iterable<A>` | Abstracción iterable posiblemente diferida; puede admitir asincronía. | No asumir materialización ni validación global; justificar su uso como estado aceptado. |
| `Fin<A>` | Resultado de una transformación o evolución que acepta o rechaza una propuesta; `Transform`/`Evolve` del modelo actual retornan `Fin<SELF>`. | Un `Fin.Fail` indica fallo de la operación, no ausencia admisible dentro de `Input`/`State`. |
| `Either<L,R>` | Alternativas de significado explícitamente diferenciadas **solo cuando el dominio las necesita**. | No duplicar el fracaso de `Fin<A>` con `Fin<Either<Error,A>>` sin una distinción real; no sustituir las firmas exigidas por traits. |
| `IO<A>`, `Eff<RT,A>`, `OptionT<IO,A>` | Operaciones dependientes del mundo y de capacidades; son pertinentes en Work/Grounding. | No introducirlas en `Transform` o `Evolve` puros para buscar evidencia que no fue proporcionada. |

**Preguntas de decisión:** Para un valor individual: ¿es válida su ausencia? Para una colección: ¿el vacío es válido? ¿inexistente y vacía significan cosas distintas? ¿la colección necesita reglas que su estructura no garantiza? ¿hay dos estados representables con el mismo significado? ¿La transformación puede rechazar una propuesta? Expresarlo en `Fin`, no mediante excepciones de control normal. ¿Hace falta consultar el exterior? Obtener evidencia antes de transformar, desde la responsabilidad correspondiente; no esconderla en Spaces. Preferir los tipos requeridos por el contrato existente a inventar combinaciones adicionales.

**Composición y versiones:** verificar la versión de LanguageExt instalada antes de aplicar `Map`, `Bind`, `Match` o LINQ, así como conversiones entre `Fin`, `K<...>` y otros tipos. Para esos detalles consultar `language-ext-development`. Los ejemplos del Framework no sustituyen la compilación contra el paquete real.

## Referencias obligadas por contexto

- Reglas actuales, alcance y límites: [docs/spaces/modeling.md](https://github.com/vslices/framework/blob/master/docs/spaces/modeling.md).
- Transformación y autoridad: [Transformable.cs](https://github.com/vslices/framework/blob/master/src/VSlices.Spaces/Traits/Transformable.cs).
- Evolución: [Evolvable.cs](https://github.com/vslices/framework/blob/master/src/VSlices.Spaces/Traits/Evolvable.cs).
- Realización concreta, **no plantilla universal**: [Todo.cs](https://github.com/vslices/framework/blob/master/examples/SampleWorkflow/SampleWorkflow.Spaces/Todo.cs).
- Verificación: [SpaceModelingLaws.cs](https://github.com/vslices/framework/blob/master/src/VSlices.Spaces/Laws/SpaceModelingLaws.cs).

## Resultado esperado

Entregar las definiciones implementadas con su construcción/transformación y, si corresponde, evolución; pruebas aplicables; un resumen de **significado, invariantes, decisiones, evidencia ejecutada y límites**; y **siempre un informe de modelado** según [modeling-report.md](references/modeling-report.md). Separar errores o incoherencias demostradas de información no especificada y convenciones provisionales. No bloquear el desarrollo ni inventar reglas porque falte información, salvo que esa ausencia impida establecer una garantía indispensable.

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
2. **Delimitar el significado.** Determinar qué representa la definición, qué identidad o igualdad le corresponde, qué evidencia necesita para existir y cuáles son sus invariantes. Distinguir datos externos de valores semánticos aceptados. No crear un tipo por cada primitivo sin justificación.
3. **Decidir construcción y opcionalidad.** Para definiciones en un namespace `Definitions` sujeto a las Laws, declarar `SELF.Input` y realizar `Transformable<SELF.Input, SELF>` con `Fin<SELF> Transform(Input)`. Encapsular constructores. Aplicar la selección de tipos funcionales de la sección siguiente: requerir `A` cuando sea obligatorio; usar `Option<A>` solo cuando `None` sea un estado de información admisible en la construcción. No utilizar nullabilidad C# en `Input`/`State` sujetos a SM007.
4. **Decidir si hay evolución.** No aplicar `Evolvable` a toda definición. Si hay estado aceptado que puede cambiar, considerar `Evolvable<SELF, SELF.State>`, `CurrentState` de solo lectura y `Evolve(old, proposed)` validando la transición sin mutar la instancia original. Proteger la identidad si no debe cambiar. Seguir la convención actual de `State` anidado y constructor privado cuando corresponda.
5. **Realizar lo mínimo.** Escribir la transformación pura con la evidencia explícita disponible. No introducir `IO`, consultas, servicios o adaptadores en `Transform` para obtener hechos faltantes. No confundir `UnsafeAccessor` (mecanismo de .NET para acceder a un constructor privado) con una garantía semántica.
6. **Probar y revisar.** Compilar. Ejecutar `SpaceModelingLaws.InspectDefinitions(assembly, definitionsNamespace)` o `Inspect(type)` en pruebas del consumidor y revisar los diagnósticos SM001–SM007. Agregar pruebas de comportamiento para aceptación/rechazo, igualdad, preservación de invariantes y transiciones admitidas/rechazadas. Las Laws son estructurales: **no prueban corrección semántica**. Declarar explícitamente lo no ejecutado o no demostrado.

## Tipos de LanguageExt dentro de Spaces

| Tipo | Papel en el modelo semántico | Límite |
| --- | --- | --- |
| `A` | Valor obligatorio de `Input` o `State` | No envolverlo en `Option<A>` por anticipar fallos o datos futuros. |
| `Option<A>` | Ausencia válida **en el contrato semántico**; `Some`/`None` tienen significado definido. | `None` no equivale a error, desconocimiento transitorio ni rechazo de construcción por defecto. |
| `Fin<A>` | Resultado de una transformación o evolución que acepta o rechaza una propuesta; `Transform`/`Evolve` del modelo actual retornan `Fin<SELF>`. | Un `Fin.Fail` indica fallo de la operación, no ausencia admisible dentro de `Input`/`State`. |
| `Either<L,R>` | Alternativas de significado explícitamente diferenciadas **solo cuando el dominio las necesita**. | No duplicar el fracaso de `Fin<A>` con `Fin<Either<Error,A>>` sin una distinción real; no sustituir las firmas exigidas por traits. |
| `IO<A>`, `Eff<RT,A>`, `OptionT<IO,A>` | Operaciones dependientes del mundo y de capacidades; son pertinentes en Work/Grounding. | No introducirlas en `Transform` o `Evolve` puros para buscar evidencia que no fue proporcionada. |

**Preguntas de decisión:** ¿puede existir el Space si este dato falta? Si no, `A`. Si sí, ¿qué significa `None`? ¿La transformación puede rechazar una propuesta? Expresarlo en `Fin`, no mediante excepciones de control normal. ¿Hace falta consultar el exterior? Obtener evidencia antes de transformar, desde la responsabilidad correspondiente; no esconderla en Spaces. Preferir los tipos requeridos por el contrato existente a inventar combinaciones adicionales.

**Composición y versiones:** verificar la versión de LanguageExt instalada antes de aplicar `Map`, `Bind`, `Match` o LINQ, así como conversiones entre `Fin`, `K<...>` y otros tipos. Para esos detalles consultar `language-ext-development`. Los ejemplos del Framework no sustituyen la compilación contra el paquete real.

## Referencias obligadas por contexto

- Reglas actuales, alcance y límites: [docs/spaces/modeling.md](https://github.com/vslices/framework/blob/master/docs/spaces/modeling.md).
- Transformación y autoridad: [Transformable.cs](https://github.com/vslices/framework/blob/master/src/VSlices.Spaces/Traits/Transformable.cs).
- Evolución: [Evolvable.cs](https://github.com/vslices/framework/blob/master/src/VSlices.Spaces/Traits/Evolvable.cs).
- Realización concreta, **no plantilla universal**: [Todo.cs](https://github.com/vslices/framework/blob/master/examples/SampleWorkflow/SampleWorkflow.Spaces/Todo.cs).
- Verificación: [SpaceModelingLaws.cs](https://github.com/vslices/framework/blob/master/src/VSlices.Spaces/Laws/SpaceModelingLaws.cs).

## Resultado esperado

Entregar las definiciones implementadas con su construcción/transformación y, si corresponde, evolución; pruebas aplicables; y un resumen breve de **significado, invariantes, decisiones, evidencia ejecutada y límites**. Si no existe autoridad conceptual suficiente, delimitar lo pendiente en lugar de atribuir significado inventado al Space.

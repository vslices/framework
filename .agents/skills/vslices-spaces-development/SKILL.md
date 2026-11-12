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

Al componer `Fin`, `Option` u otras abstracciones funcionales, consultar además `language-ext-development` para verificar semántica y APIs de LanguageExt.

## Procedimiento mínimo

1. **Localizar la autoridad.** Identificar el repositorio y la versión efectiva de `VSlices.Spaces`; leer el código y los contratos instalados, no confiar en ejemplos desactualizados. Consultar [Modeling semantic Spaces](https://github.com/vslices/framework/blob/master/docs/spaces/modeling.md) y los traits concretos.
2. **Delimitar el significado.** Determinar qué representa la definición, qué identidad o igualdad le corresponde, qué evidencia necesita para existir y cuáles son sus invariantes. Distinguir datos externos de valores semánticos aceptados. No crear un tipo por cada primitivo sin justificación.
3. **Decidir construcción y opcionalidad.** Para definiciones en un namespace `Definitions` sujeto a las Laws, declarar `SELF.Input` y realizar `Transformable<SELF.Input, SELF>` con `Fin<SELF> Transform(Input)`. Encapsular los constructores. Usar `A` cuando el valor es obligatorio y `Option<A>` solo cuando `None` tiene significado admisible en el momento de construcción. No utilizar nullabilidad C# en `Input`/`State` semánticos sujetos a SM007.
4. **Decidir si hay evolución.** No aplicar `Evolvable` a toda definición. Si hay estado aceptado que puede cambiar, considerar `Evolvable<SELF, SELF.State>`, `CurrentState` de solo lectura y `Evolve(old, proposed)` validando la transición sin mutar la instancia original. Proteger la identidad si no debe cambiar. Seguir la convención actual de `State` anidado y constructor privado cuando corresponda.
5. **Realizar lo mínimo.** Escribir la transformación pura con la evidencia explícita disponible. No introducir `IO`, consultas, servicios o adaptadores en `Transform` para obtener hechos faltantes. No confundir `UnsafeAccessor` (mecanismo de .NET para acceder a un constructor privado) con una garantía semántica.
6. **Probar y revisar.** Compilar. Ejecutar `SpaceModelingLaws.InspectDefinitions(assembly, definitionsNamespace)` o `Inspect(type)` en pruebas del consumidor y revisar los diagnósticos SM001–SM007. Agregar pruebas de comportamiento para aceptación/rechazo, igualdad, preservación de invariantes y transiciones admitidas/rechazadas. Las Laws son estructurales: **no prueban corrección semántica**. Declarar explícitamente lo no ejecutado o no demostrado.

## Referencias obligadas por contexto

- Reglas actuales, alcance y límites: [docs/spaces/modeling.md](https://github.com/vslices/framework/blob/master/docs/spaces/modeling.md).
- Transformación y autoridad: [Transformable.cs](https://github.com/vslices/framework/blob/master/src/VSlices.Spaces/Traits/Transformable.cs).
- Evolución: [Evolvable.cs](https://github.com/vslices/framework/blob/master/src/VSlices.Spaces/Traits/Evolvable.cs).
- Realización concreta, **no plantilla universal**: [Todo.cs](https://github.com/vslices/framework/blob/master/examples/SampleWorkflow/SampleWorkflow.Spaces/Todo.cs).
- Verificación: [SpaceModelingLaws.cs](https://github.com/vslices/framework/blob/master/src/VSlices.Spaces/Laws/SpaceModelingLaws.cs).

## Resultado esperado

Entregar las definiciones implementadas con su construcción/transformación y, si corresponde, evolución; pruebas aplicables; y un resumen breve de **significado, invariantes, decisiones, evidencia ejecutada y límites**. Si no existe autoridad conceptual suficiente, delimitar lo pendiente en lugar de atribuir significado inventado al Space.

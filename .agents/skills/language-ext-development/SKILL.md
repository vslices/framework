---
name: language-ext-development
description: >-
  Implementar, corregir y revisar código C# con LanguageExt 5, especialmente
  Fin, Option, Either, IO, Eff, transformadores y composición LINQ. Usar al
  introducir o modificar operaciones funcionales, combinar efectos, modelar
  errores/ausencia o resolver problemas de tipos y sobrecargas en VSlices y
  otros proyectos que usan LanguageExt. Verificar siempre versión y firmas reales.
---

# Desarrollo con LanguageExt

## Objetivo

Escribir código idiomático, legible y comprobable usando **la versión efectiva** de LanguageExt del proyecto. **LanguageExt es una dependencia conceptual deliberada de VSlices**, además de una dependencia técnica de su realización .NET: el modelo funcional forma parte de la orientación de diseño y la evolución prevista incluye desarrollar un fork propio de LanguageExt sobre el que se construirá VSlices. **Ese fork es una dirección futura, no una dependencia ya implementada**; hasta su existencia y adopción verificadas, trabajar con las APIs del paquete realmente instalado. Esta Skill enseña sus mecanismos funcionales y su uso dentro de VSlices; las decisiones concretas de invariantes, propiedad semántica y responsabilidades siguen perteneciendo a los contratos de VSlices. Si el problema es definir un Space, seguir además `vslices-spaces-development`; para Features y Grounding, respetar sus contratos propietarios.

## Procedimiento mínimo

1. **Identificar la versión real.** Leer el `PackageReference`, lockfile o dependencia transitiva y el código que efectivamente compila. En `vslices/framework` (al crear esta Skill) `src/VSlices/VSlices.csproj` referencia `LanguageExt.Core 5.0.0-beta-78`; tratarlo como evidencia localizada, no como versión universal.
2. **Clasificar el resultado y el contexto.** Separar valor puro, ausencia admisible (`Option<A>`), resultado fallible (`Fin<A>` o `Either<L,R>` según el contrato), operación de mundo (`IO<A>`) y ejecución dependiente de un runtime (`Eff<RT,A>`). No apilar wrappers que dupliquen un mismo canal de fallo.
3. **Elegir la composición mínima.** Para combinar `Fin<A>` y `Eff<RT,B>`, revisar las extensiones LINQ de VSlices en ambos sentidos (`Fin.Monad.Eff2.cs` y `Eff2.Monad.Fin.cs`); una expresión puede empezar con `Fin` y continuar con `Eff` sin envolver automáticamente el `Fin` en `liftEff`. Esto evalúa la primera expresión `Fin` al construir la consulta, a diferencia de `liftEff(() => ...)`, que difiere la evaluación. Consultar [validación con Fin](references/fin-composition.md) y verificar compilación y momento de evaluación. Para `Fin<A>`, distinguir validaciones independientes —combinar de forma aplicativa y acumular errores— de validaciones dependientes —encadenar monádicamente y propagar fallo—; consultar [validación con Fin](references/fin-composition.md). Preferir `Map`, `Bind`, pattern matching y LINQ cuando expresen directamente el flujo; utilizar `Match` cuando realmente se necesite examinar ramas, no anidar múltiples `Match` para simular composición. Mantener puros los cálculos puros y explícitos los efectos. Antes de usar `LiftIO`, `liftEff`, `Run`, `As` u otras conversiones, inspeccionar los tipos y sobrecargas concretas. Para un downcast final compatible con LanguageExt v5, preferir el operador unario `+` en lugar de `.As()` **solo cuando la conversión sea la última instrucción efectiva del método**; consultar [operadores de LanguageExt v5](references/v5-operators.md).
4. **Respetar los tipos.** Comprobar si una API devuelve `A`, `Option<A>`, `Fin<A>`, `IO<A>`, `OptionT<IO,A>`, `OptionT<Eff<RT>,A>` o una forma `K<F,A>`: no asumir que todas las operaciones disponibles sobre los tipos concretos están disponibles sobre `K`. En una consulta LINQ, examinar especialmente el tipo de la primera fuente y las sobrecargas de `SelectMany`. Para las dos formas de `OptionT`, consultar [transformadores de ausencia](references/optiont-effects.md): VSlices ofrece `OptionT<M,A>.IfNone(Func<Error>)` cuando `M : Monad<M>, Fallible<M>` para interpretar explícitamente `None` como fallo. No convertir `None` en error por rutina; esta extensión está en el PR #197, no asumir que ya existe en un NuGet instalado.
5. **Verificar en el consumidor.** Compilar con la versión instalada y probar al menos el caso exitoso, ausencia si aplica, rechazo esperado y propagación de errores/efectos. Confirmar el comportamiento de errores simultáneos cuando se promete acumulación. Si el compilador contradice un ejemplo, corregir el ejemplo desde las firmas verificadas, no introducir conversiones a ciegas. Usar ejemplos autónomos y fuentes públicas accesibles; no incorporar referencias procedentes de repositorios privados sin revisión humana previa.

## Decisiones rápidas

| Situación | Examinar |
| --- | --- |
| Dato opcional semánticamente válido | `Option<A>`; definir qué significa `None` |
| Transformación que puede rechazar un valor | `Fin<A>` o contrato fallible existente |
| Error tipado específico de una alternativa | `Either<L,R>`, solo si el contrato necesita distinguirla |
| Lectura/escritura externa | `IO<A>` y, cuando hay ausencia, `OptionT<IO,A>` |
| Lectura opcional dependiente del runtime | `OptionT<Eff<RT>,A>` si la ausencia conserva significado y el runtime aporta capacidades |
| Validaciones independientes | `Fin<A>` con aplicación que acumule errores; comprobar la operación real |
| Validaciones dependientes | `Fin<A>` con `Bind` o LINQ; no prometer acumulación de errores no evaluados |
| Efecto que necesita capacidades del runtime | `Eff<RT,A>` |
| Secuencia de operaciones | LINQ o `Bind`, verificando sobrecargas disponibles |

No usar estas correspondencias como una taxonomía absoluta; las firmas y el significado del contrato prevalecen.

## Composición de errores y efectos

- Las extensiones de composición `Fin ↔ Eff<RT>` son código de VSlices en el namespace `LanguageExt`, no capacidades que deba atribuirse automáticamente al paquete LanguageExt puro. Consultar [Fin: applicative y monad](references/fin-composition.md) para distinguir validaciones simultáneas y encadenadas, y preservar errores propios de cada invariante.
- Consultar [OptionT con IO y Eff](references/optiont-effects.md) para componer ausencia y efectos sin colapsar prematuramente los canales.
- Consultar [operadores y downcasts de LanguageExt v5](references/v5-operators.md). No incorporar extensiones particulares de otros proyectos sin revisar sus contratos, la API estándar vigente y la pertinencia para VSlices.
- No recomendar universalmente convertir `None` en fallo de `Eff` por el caso aislado de Approval; respetar primero las convenciones del Framework y requerir nueva evidencia.
- Utilizar sintaxis moderna de C# compatible con el proyecto (pattern matching y expresiones de colección para `Seq<A>` cuando compile en v5); no extrapolar restricciones históricas de LanguageExt v4.

## Fuentes y ejemplos verificables

- [Dependencia efectiva de VSlices](https://github.com/vslices/framework/blob/master/src/VSlices/VSlices.csproj).
- [LanguageExt, repositorio propietario](https://github.com/louthy/language-ext) (usar documentación y código que correspondan a la versión instalada).
- [Point Algebras](https://github.com/vslices/framework/blob/master/docs/point-algebras.md), para `IO` y `OptionT<IO,A>`.
- [Feature Model](https://github.com/vslices/framework/blob/master/docs/feature-model.md), para `Eff<ALG, Response>` en VSlices.
- [Spaces Modeling](https://github.com/vslices/framework/blob/master/docs/spaces/modeling.md), para fallos de construcción y opcionalidad.
- [LanguageExt v5 beta-56](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-56) y [beta-57](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-57), para operadores de extensión y downcast.

## Entrega

Producir código que compile o declarar claramente si no se pudo ejecutar la compilación; señalar el contrato funcional elegido y por qué, los casos de éxito/ausencia/fallo pertinentes y cualquier diferencia entre ejemplos consultados y APIs efectivamente disponibles.

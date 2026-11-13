# Composición de efectos y ejecución de Features

## Fronteras

- `Fin<A>` representa una admisibilidad o transformación pura fallible.
- `IO<A>` expresa contacto con el exterior.
- `Eff<ALG,A>` expresa ejecución dependiente del Algebra.
- `OptionT<IO,A>` expresa lectura opcional efectful; `FinT<M,A>` es el resultado fallible transformado requerido por `ValidatableM` y `TransformableM`.

Mantener la evidencia obtenida por Grounding separada de las reglas puras del Space. Una Feature decide el uso contextual y orquesta los efectos. Para APIs concretas de `Bind`, `Map`, `LiftIO`, `Run`, `As` y `SelectMany`, consultar la versión instalada de LanguageExt v5.

## Ejemplo oficial del Framework

La documentación de [Feature Model](https://github.com/vslices/framework/blob/master/docs/feature-model.md) presenta `ReadItem.Get(Request)` devolviendo `Eff<ItemAlgebra,Response>` mediante `Eff<...>.LiftIO(algebra => algebra.Reader.Read(request.Id).Map(NewResponse))`.

La documentación de [Point Algebras](https://github.com/vslices/framework/blob/master/docs/point-algebras.md) presenta un caso opcional con `ReadOrDefault(request.Id).Run().As().Map(NewResponse)`.

## Composición entre módulos

```csharp
// Extracto ilustrativo de documentación actual de Framework:
Child.Get(new Child.Request(...)).RunIO(mix.A)
```

Al componer, construir explícitamente el Request del hijo, proyectar el Algebra que exige y tratar el resultado conforme a su contrato. No trasladar al parent la propiedad del Response del hijo.

Probar la diferencia entre obtener un `Eff` y ejecutarlo; comprobar ausencia, errores de validación y efectos externos. No asumir transacción distribuida, rollback o completitud por el mero hecho de usar `AlgebraMix`.

## Fin y Eff: composición LINQ en ambos sentidos

Las extensiones propias de VSlices (no necesariamente de LanguageExt Core sin VSlices) en `src/VSlices/Monads/Fin/Syntax/Monad/Fin.Monad.Eff2.cs` y `src/VSlices/Monads/Eff/Syntax/Monad/Eff2.Monad.Fin.cs` implementan `SelectMany`/`Bind` para `Fin<A> → Eff<ALG,B>` y `Eff<ALG,A> → Fin<B>`. La consulta puede empezar por la transformación semántica y pasar después a obtener capacidades del runtime:

```csharp
Eff<Alg, Response> Get(Request request) =>
    from item in Item.Transform(new(request.Id, request.Name)) // Fin<Item>
    from alg in runtime<Alg>()
    from stored in alg.Reader.Read(item.Id)
    select NewResponse(stored);
```

Esto no exige un `ToEff<ALG>()` explícito para la transición de `Fin` a `Eff`. Pero `Item.Transform` en la **primera fuente** se evalúa durante `Get(request)`; `liftEff(() => Item.Transform(...))` difiere la evaluación hasta ejecutar el `Eff`. Mantener la diferencia si importa el momento de validar, fallar o lanzar excepciones. Comprobar el caso concreto por compilación.

## Interpretación explícita de None como error

VSlices Framework PR #197 incorpora `OptionT<M,A>.IfNone(Func<Error>)`, con restricciones `M : Monad<M>, Fallible<M>` y retorno `K<M,A>`. Interpreta `Some(a)` como `M.Pure(a)`, `None` como `M.Fail<A>(fe())` y propaga cualquier fallo anterior del efecto. El `Error` se calcula solo en la rama `None`.

Usar esta operación **solo cuando la ausencia sea un rechazo esperado**, por ejemplo cuando una Feature exige un punto cuya inexistencia debe fallar con error específico. No usarla para lecturas opcionales legítimas; no deducir garantías transaccionales ni de existencia permanente. Revisar primero si la versión de VSlices del consumidor incluye la extensión: todavía está en PR #197, no en los paquetes previamente publicados. Consultar también `language-ext-development/references/optiont-effects.md`.

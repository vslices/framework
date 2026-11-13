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

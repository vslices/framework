# OptionT con IO y Eff

## Separación de significados

- `Option<A>`: ausencia válida en un cálculo puro.
- `OptionT<IO,A>`: lectura u operación con efectos que puede producir `None`.
- `OptionT<Eff<RT>,A>`: lectura opcional que necesita las capacidades del runtime `RT`.
- `Eff<RT,A>`: efecto que produce un valor requerido o falla. No convertir `None` en fallo automáticamente; esa decisión pertenece al contrato consumidor.

Consultar los tipos de retorno antes de combinar `K<F,A>`, `IO<A>`, `Eff<RT,A>` y transformadores. En LINQ, la primera fuente condiciona las sobrecargas disponibles de `SelectMany`.

## Referencia VSlices (OptionT<IO,A>)

[`PointReader<POINT,ID>.ReadOrDefault`](https://github.com/vslices/framework/blob/master/docs/point-algebras.md) retorna `OptionT<IO,POINT>`; es una lectura opcional con efecto. En el ejemplo de `GetTodo`, `ReadOrDefault(id).Run().As().Map(NewResponse)` preserva la opción hasta construir la respuesta. `Read` es la vista de valor requerido del contrato, no la representación obligatoria de todo lookup.

## Extracción y conversión

`Run()` extrae el transformador a un efecto con `Option<A>`; `Match` puede decidir entonces qué hacer con `Some` o `None`. Utilizar `As()` únicamente cuando sea compatible con los tipos concretos retornados; si el downcast es la última instrucción efectiva y el tipo admite el operador unario de LanguageExt v5, preferir `+` (véase [operadores](v5-operators.md)). No asumir que `OptionT<IO,A>` y `OptionT<Eff<RT>,A>` tienen idénticas conversiones.

La ausencia puede ser respuesta válida, no encontrado, condición de una alternativa o requisito incumplido, según contrato. Evitar reglas universales sobre cuándo fallar con `Eff`; la observación previa sobre Approval no basta para establecerlas.

## Convertir ausencia en error de manera explícita (VSlices PR #197)

Cuando `None` signifique **rechazo** para la operación consumidora, VSlices incorpora en `src/VSlices/Monads/OptionT/Syntax/OptionT.Fallible.cs` la extensión:

```csharp
K<M, A> IfNone<M, A>(this OptionT<M, A> optional, Func<Error> fe)
    where M : Monad<M>, Fallible<M>;
```

La implementación emplea `optional.Run().Bind(option => option.Match(Some: M.Pure, None: () => M.Fail<A>(fe())))`. El `Some(a)` se conserva como `M.Pure(a)`; `None` invoca `fe()` y produce `M.Fail<A>`; un fallo previo de `M` se propaga sin entrar al `Match`. Se requieren **ambos** traits: `Monad<M>` para `Bind`/`Pure` y `Fallible<M>` para `Fail`.

La fábrica `fe` se evalúa **únicamente si** el `Option` observado es `None` y en el momento en que el monad exterior evalúe ese paso. Para `OptionT<Eff<RT>,A>`, esto mantiene el fallo diferido a la ejecución del efecto. No fuerza `None` a error cuando la ausencia sea un resultado válido; continuar con `OptionT` o con `Option<A>` en ese caso. Pruebas de `Fin` y de `Eff<RT>` se encuentran en `tests/VSlices.Work.Algebra.Tests/OptionTFallibleTests.cs`.

**Disponibilidad:** esta extensión pertenece al trabajo no fusionado del PR #197 de VSlices. Confirmar versión/commit del Framework; no atribuirla a `LanguageExt.Core 5.0.0-beta-78` ni a la versión NuGet anterior de VSlices.

## Pruebas sugeridas

Verificar `Some`, `None`, fallo de efecto, orden/laziness de operaciones y propagación de errores de la realización, con el runtime que corresponda.

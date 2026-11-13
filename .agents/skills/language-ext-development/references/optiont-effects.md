# OptionT con IO y Eff

## Separación de significados

- `Option<A>`: ausencia válida en un cálculo puro.
- `OptionT<IO,A>`: lectura u operación con efectos que puede producir `None`.
- `OptionT<Eff<RT>,A>`: lectura opcional que necesita las capacidades del runtime `RT`.
- `Eff<RT,A>`: efecto que produce un valor requerido o falla. No convertir `None` en fallo automáticamente; esa decisión pertenece al contrato consumidor.

Consultar los tipos de retorno antes de combinar `K<F,A>`, `IO<A>`, `Eff<RT,A>` y transformadores. En LINQ, la primera fuente condiciona las sobrecargas disponibles de `SelectMany`.

## Referencia VSlices (OptionT<IO,A>)

[`PointReader<POINT,ID>.ReadOrDefault`](https://github.com/vslices/framework/blob/master/docs/point-algebras.md) retorna `OptionT<IO,POINT>`; es una lectura opcional con efecto. En el ejemplo de `GetTodo`, `ReadOrDefault(id).Run().As().Map(NewResponse)` preserva la opción hasta construir la respuesta. `Read` es la vista de valor requerido del contrato, no la representación obligatoria de todo lookup.

## Referencia Hual (OptionT<Eff<RT>,A>)

[`PdiBook.Module.cs`](https://github.com/HernanFAR/Hual/blob/master/Legals.Domain/Aggregates/PdiBook/PdiBook.Module.cs) utiliza:

```csharp
public static OptionT<Eff<HualRuntime>, PdiBook> SearchInRepository(this PdiBookCode code) =>
    from repository in provide<IPdiBookRepository>()
    from book in repository.Read(code)
    select book;
```

La obtención de una capacidad del runtime y la lectura opcional se componen conservando el canal de ausencia. Esta sintaxis pertenece a la realización observada en Hual; verificar versiones y sobrecargas antes de trasplantar.

## Extracción y conversión

`Run()` extrae el transformador a un efecto con `Option<A>`; `Match` puede decidir entonces qué hacer con `Some` o `None`. Utilizar `As()` únicamente cuando sea compatible con los tipos concretos retornados. No asumir que `OptionT<IO,A>` y `OptionT<Eff<RT>,A>` tienen idénticas conversiones.

La ausencia puede ser respuesta válida, no encontrado, condición de una alternativa o requisito incumplido, según contrato. Evitar reglas universales sobre cuándo fallar con `Eff`; la observación previa sobre Approval no basta para establecerlas.

## Pruebas sugeridas

Verificar `Some`, `None`, fallo de efecto, orden/laziness de operaciones y propagación de errores de la realización, con el runtime que corresponda.

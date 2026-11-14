# Fin<A>: composición aplicativa y monádica

## Dos dependencias distintas

- **Applicative:** varias validaciones independientes pueden evaluarse y luego combinarse, preservando cada error. Esto es especialmente importante al construir un Space con propiedades que tienen invariantes propias.
- **Monad:** una validación depende del resultado de otra. `Bind` o `from` encadenan las operaciones y detienen la continuación dependiente al fallar una anterior.
- **Combinación:** validar independientemente las propiedades primero; después comprobar una relación entre los valores aceptados.

## Ejemplo de combinación aplicativa

Cuatro validaciones independientes producen `Fin<TareId>`, `Fin<string>`, `Fin<string>` y `Fin<NonNegativeKilogram>`. La combinación puede escribirse así:

```csharp
return +(idVal, detailVal, referencedNameVal, massVal)
    .Apply((id, detail, refName, mass) => new Tare(id, detail, refName, mass));
```

El `+` unario sustituye el `.As()` de cierre cuando ese downcast es la **última instrucción efectiva del método**, y la versión instalada admite el operador para el tipo devuelto. Para prometer **acumulación**, ejecutar pruebas con dos o más errores independientes: no inferirla solo por encontrar `.Apply`. Véase [operadores de LanguageExt v5](v5-operators.md).

## Dependencia semántica (conceptual)

```csharp
Fin<(StartDate, EndDate)> dates =
    from start in StartDate.Transform(input.StartDate)
    from end in EndDate.Transform(input.EndDate)
    from _ in start < end
        ? Fin.Succ(unit)
        : Fin.Fail<Unit>(Error.New("Start debe ser anterior a End"))
    select (start, end);
```

Este ejemplo ilustra dependencia monádica: la comparación no se puede hacer antes de obtener las dos fechas. No demuestra acumulación aplicativa de los errores de construcción de cada fecha. Verificar los tipos y operadores contra el paquete instalado antes de afirmar que el fragmento compila.

## Revisión de invariantes

1. Relacionar cada fallo con su propiedad/invariante; evitar sustituir múltiples errores independientes por uno genérico.
2. Distinguir lo que puede evaluarse independientemente de lo que requiere un resultado anterior.
3. Probar aceptación, cada rechazo aislado y fallos simultáneos para confirmar comportamiento y atribución.
4. Usar `Map` para proyectar éxito, `Bind`/LINQ para dependencias y `Match` para interpretar ambos resultados cuando sea necesario; no sustituir composición por `Match` anidados.
5. Evitar añadir operadores o abstracciones propias si la API instalada ya expresa el contrato.

## Cruce con Eff<RT> en VSlices

VSlices aporta `Fin.Monad.Eff2.cs` y `Eff2.Monad.Fin.cs` (namespace `LanguageExt`) con `Bind` y `SelectMany` en ambos sentidos. La primera fuente de una consulta LINQ **puede ser `Fin<A>`** y su siguiente paso `Eff<RT,B>`; `SelectMany` devuelve `Eff<RT,C>`, conservando el fracaso de `Fin` sin construir manualmente un `liftEff`:

```csharp
Eff<Alg, Response> Get(Request request) =>
    from item in Item.Transform(new(request.Id, request.Name)) // Fin<Item>
    from alg in runtime<Alg>()                             // Eff<Alg, Alg>
    from stored in alg.Reader.Read(item.Id)                 // IO<Item> (composición posterior)
    select NewResponse(stored);
```

**Semántica temporal:** la primera llamada a `Item.Transform` se evalúa cuando se invoca `Get`, mientras que `liftEff(() => Item.Transform(...))` la evalúa al ejecutar el efecto. No asumir equivalencia temporal o de excepciones; estas extensiones solo gobiernan la composición de los resultados. En el sentido inverso, `Eff<RT,A>` puede continuar con un `Fin<B>` mediante las sobrecargas de VSlices, preservando el runtime `RT`. Comprobar las firmas efectivas y compilar en el consumidor; no extrapolar estas extensiones al paquete LanguageExt sin VSlices.

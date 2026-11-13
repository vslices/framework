# Fin<A>: composición aplicativa y monádica

## Dos dependencias distintas

- **Applicative:** varias validaciones independientes pueden evaluarse y luego combinarse, preservando cada error. Esto es especialmente importante al construir un Space con propiedades que tienen invariantes propias.
- **Monad:** una validación depende del resultado de otra. `Bind` o `from` encadenan las operaciones y detienen la continuación dependiente al fallar una anterior.
- **Combinación:** validar independientemente las propiedades primero; después comprobar una relación entre los valores aceptados.

## Evidencia concreta: Hual / Tare

En [`Tare.New`](https://github.com/HernanFAR/Hual/blob/master/LogisticsContext.Domain/Aggregates/Tare.cs), cuatro validaciones producen `Fin<TareId>`, `Fin<string>`, `Fin<string>` y `Fin<NonNegativeKilogram>`, y se combinan así:

```csharp
return (idVal, detailVal, referencedNameVal, massVal)
    .Apply((id, detail, refName, mass) => new Tare(id, detail, refName, mass))
    .As();
```

Es evidencia de la sintaxis utilizada en Hual; comprobar la versión de paquete que efectivamente compila ese consumidor antes de copiarla. Cuando se requiere **acumulación**, ejecutar pruebas con dos o más errores independientes: no inferirla solo por encontrar `.Apply`.

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

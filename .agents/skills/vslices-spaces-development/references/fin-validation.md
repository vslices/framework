# Validaciones de Spaces con Fin<A>

## Responsabilidad

Cada propiedad e invariante mantiene su autoridad semántica y sus errores concretos. Un fallo de `ActivityName` no debe transformarse en un mensaje genérico de `Activity`, ni perderse al validar otra propiedad.

## Dos formas complementarias

- **Composición aplicativa:** cuando varias validaciones no dependen unas de otras, evaluarlas y combinar los resultados de modo que puedan acumularse sus errores. Una sola falla genérica para todas las propiedades elimina información útil. Confirmar la API concreta de aplicación/traversal contra LanguageExt v5 instalado y probar que efectivamente acumula errores (no toda combinación de `Fin` los acumula).
- **Composición monádica:** cuando una validación necesita el valor obtenido por la anterior, encadenar mediante `Bind` o sintaxis LINQ. El fracaso de una dependencia impide continuar esa rama; esto no sustituye la acumulación de validaciones independientes.

Ambos enfoques pueden mezclarse: primero validar propiedades independientes y luego comprobar una relación que requiere los valores válidos.

## Ejemplo conceptual de dependencia

```csharp
// El predicado se evalúa solo cuando ambas transformaciones tienen éxito.
// Verificar operadores y conversiones exactos contra LanguageExt v5 instalado.
Fin<(StartDate Start, EndDate End)> dates =
    from start in StartDate.Transform(input.StartDate)
    from end in EndDate.Transform(input.EndDate)
    from _ in start < end
        ? Fin.Succ(unit)
        : Fin.Fail<Unit>(Error.New("Start debe ser anterior a End"))
    select (start, end);
```

Esta consulta es *monádica*: no prueba acumulación de fallos independientes de fecha. Para esa garantía, aplicar primero una composición aplicativa cuya semántica de acumulación esté comprobada en la versión instalada. No presentar este esbozo como código compilado.

## Criterios de revisión

1. Asociar errores específicos a cada propiedad y cada invariante violada.
2. Distinguir validaciones independientes de aquellas que requieren resultados previos.
3. Preferir composición declarativa y pattern matching sobre `Match` profundamente anidados, sin prohibir `Match` cuando aclara decisiones.
4. No ocultar errores específicos detrás de `Error.New("Invalid input")` general.
5. Probar fallas aisladas y simultáneas; comprobar el número y la atribución de los errores.
6. Conservar reglas del dominio en Spaces y adquisición de información con efectos en Work/Grounding.
7. Evitar abstracciones de validación adicionales sin presión observable.

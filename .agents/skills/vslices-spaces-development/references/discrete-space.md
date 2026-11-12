# DiscreteSpace: igualdad semántica

Antes de implementar, consultar `src/VSlices.Spaces/DiscreteSpace.cs` del Framework vigente. Su contrato actual es:

```csharp
public interface DiscreteSpace<SELF> :
    IEquatable<SELF>,
    IEqualityOperators<SELF, SELF, bool>
    where SELF : DiscreteSpace<SELF>;
```

`DiscreteSpace` declara **igualdad semántica**. No añade orden, suma, transformación ni ninguna otra estructura algebraica. El tipo es responsable de determinar qué significa que dos habitantes del Space sean iguales.

## Elegir la igualdad por significado

1. **Identidad:** dos puntos del mismo concepto pueden tener estados distintos y compartir identidad. Por ejemplo, dos versiones de una actividad con el mismo `ActivityId` pueden ser iguales como actividades, sin que sus estados sean idénticos. No usar esa igualdad para verificar que un estado histórico no cambió.
2. **Valor:** dos cantidades, nombres u otros valores pueden ser iguales por sus características semánticamente relevantes. No limitarse al backing `string` o `Guid` cuando el contexto exige otra interpretación.
3. **Equivalencia semántica:** la normalización puede participar de la igualdad si el dominio define que valores con distintas representaciones son equivalentes; una clave normalizada de búsqueda **no cambia automáticamente** la igualdad del Space. Distinguir comparación para búsqueda, reglas de unicidad contextual e igualdad de valores.
4. **Colecciones:** examinar si el concepto exige igualdad estructural por contenido, con o sin sensibilidad al orden, identidad propia, o si la igualdad no debe utilizarse como regla de invariantes. Un `record` que contenga `ImmutableArray<T>` **no garantiza automáticamente** comparación elemento a elemento por contenido; implementar la semántica requerida explícitamente.

## Realizar la igualdad

- Implementar `Equals(SELF?)`, `GetHashCode`, operadores `==` y `!=`, y `Equals(object?)` cuando corresponda, manteniendo una misma relación de igualdad. Para `record`, revisar la igualdad generada; no duplicarla innecesariamente ni asumir que coincide con la relación semántica.
- Si dos valores son iguales, **sus códigos hash deben coincidir**. Los hashes iguales no obligan a que los valores sean iguales.
- Mantener la relación reflexiva, simétrica y transitiva; comprobar consistencia con los operadores, incluso con null donde corresponda.
- No incorporar reglas globales como "todos los IDs son iguales por su representación" ni "todo Space compuesto es igual por identidad". Determinarlo según el dominio.
- No confundir `DiscreteSpace` con `MaintainedSpace`: este último agrega el conjunto completo `All` de constantes reconocidas y hereda igualdad de `DiscreteSpace`.

## Pruebas mínimas

- Igualdad reflexiva, simétrica y transitiva, y separación de dos valores distintos.
- Consistencia entre `Equals`, `==`, `!=` y `GetHashCode`, incluidos los casos de null admitidos por los operadores.
- Para un Space con identidad: dos puntos con el mismo ID y distintos estados son iguales **solo si** así se definió su igualdad; probar por separado las invariantes de continuidad.
- Para valores normalizados: ejemplos positivos y negativos de equivalencia conforme a reglas realmente acordadas; no añadir normalizaciones por conveniencia.
- Para colecciones: probar igualdad sobre instancias independientes con el mismo contenido, diferencia de orden si corresponde, y diferencias de elementos.
- Para `MaintainedSpace`: comprobar que `All` no contiene duplicados bajo la igualdad elegida.

## Señales de alerta

- Comparar estados completos usando la igualdad por identidad del Space propietario.
- Usar igualdad predeterminada de una colección sin verificar su semántica.
- Hacer que una comparación de búsqueda o un ámbito de unicidad cambie implícitamente el significado de `Equals`.
- Introducir ordenamiento, `IComparable` o aritmética solo por haber implementado igualdad.

# Operadores de extensión de LanguageExt v5

Las notas oficiales de [v5.0.0-beta-56](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-56) introducen operadores genéricos de extensiones en .NET 10 para traits como Functor, Applicative, Monad, Choice y Fallible; también presentan operadores de *downcast* específicos de tipos, previamente realizados con `.As()`. Las notas de [v5.0.0-beta-57](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-57) amplían la cobertura a tipos concretos, incluyendo `Fin<A>`, `FinT<M,A>`, `Option<A>`, `OptionT<M,A>`, `IO<A>`, `Eff<RT,A>` y otros. La disponibilidad y el tipo de retorno deben comprobarse contra la versión instalada.

## Downcast final: operador unario +

Cuando una expresión finaliza con `.As()` para convertir de `K<F,A>` a un tipo concreto compatible, **preferir `+expression` si esa conversión es la última instrucción efectiva del método**. El operador es una alternativa de lectura al downcast, no un paso adicional de cómputo ni una sustitución indiscriminada de cualquier llamada a `.As()`.

Ejemplo ilustrativo:

```csharp
return +(idVal, detailVal, referencedNameVal, massVal)
    .Apply((id, detail, refName, mass) => new Tare(id, detail, refName, mass));
```

Comparar con la forma equivalente cuando el tipo y la versión lo admiten:

```csharp
return (idVal, detailVal, referencedNameVal, massVal)
    .Apply((id, detail, refName, mass) => new Tare(id, detail, refName, mass))
    .As();
```

No reemplazar `.As()` cuando su resultado se necesita en una instrucción posterior, cuando otra sobrecarga exige una conversión intermedia, o cuando el operador no está disponible para ese tipo. Respetar la precedencia de C#; usar paréntesis cuando sean necesarios para conservar el alcance de la expresión.

## Otros operadores que vale la pena reconocer

- `f * fa`: mapeo mediante el operador de Functor, cuando la versión y los tipos concretos lo admiten.
- `f * fa * fb`: aplicación de una función a valores en contextos aplicativos; revisar inferencia y tipos de parámetros.
- `ma >> mb`: secuenciación monádica; con una función a la derecha puede representar `Bind`.
- `ma | fallback`: alternativa o manejo fallible, según el trait involucrado; no confundir con evaluación siempre eager.
- `+` binario: combinación de Semigroup/SemigroupK; **no confundirlo con el prefijo unario** de downcast.

Son mecanismos disponibles en LanguageExt v5, no obligaciones de estilo universales. Favorecer legibilidad y precisión semántica por encima de compactar toda composición en operadores.

## Verificación

Inspeccionar la versión real de LanguageExt, el tipo estático de la expresión y sus sobrecargas. Confirmar por compilación que el operador devuelve el mismo tipo concreto esperado y que no altera la propagación de errores/efectos. Los ejemplos aquí son ilustrativos, no se presentan como compilados en este cambio documental.

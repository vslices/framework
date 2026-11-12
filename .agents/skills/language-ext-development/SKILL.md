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

Escribir código idiomático, legible y comprobable usando **la versión efectiva** de LanguageExt del proyecto. Esta Skill enseña mecanismos funcionales, **no decide** los significados, invariantes o autoridades de VSlices. Si el problema es definir un Space, seguir además `vslices-spaces-development`; para Features y Grounding, respetar sus contratos propietarios.

## Procedimiento mínimo

1. **Identificar la versión real.** Leer el `PackageReference`, lockfile o dependencia transitiva y el código que efectivamente compila. En `vslices/framework` (al crear esta Skill) `src/VSlices/VSlices.csproj` referencia `LanguageExt.Core 5.0.0-beta-78`; tratarlo como evidencia localizada, no como versión universal.
2. **Clasificar el resultado y el contexto.** Separar valor puro, ausencia admisible (`Option<A>`), resultado fallible (`Fin<A>` o `Either<L,R>` según el contrato), operación de mundo (`IO<A>`) y ejecución dependiente de un runtime (`Eff<RT,A>`). No apilar wrappers que dupliquen un mismo canal de fallo.
3. **Elegir la composición mínima.** Preferir `Map`, `Bind`, `Match` y LINQ cuando expresen directamente el flujo. Mantener puros los cálculos puros y explícitos los efectos. Antes de usar `LiftIO`, `liftEff`, `Run`, `As` u otras conversiones, inspeccionar los tipos y sobrecargas concretas.
4. **Respetar los tipos.** Comprobar si una API devuelve `A`, `Option<A>`, `Fin<A>`, `IO<A>`, `OptionT<IO,A>` o una forma `K<F,A>`: no asumir que todas las operaciones disponibles sobre los tipos concretos están disponibles sobre `K`. En una consulta LINQ, examinar especialmente el tipo de la primera fuente y las sobrecargas de `SelectMany`.
5. **Verificar en el consumidor.** Compilar con la versión instalada y probar al menos el caso exitoso, ausencia si aplica, rechazo esperado y propagación de errores/efectos. Si el compilador contradice un ejemplo, corregir el ejemplo desde las firmas verificadas, no introducir conversiones a ciegas.

## Decisiones rápidas

| Situación | Examinar |
| --- | --- |
| Dato opcional semánticamente válido | `Option<A>`; definir qué significa `None` |
| Transformación que puede rechazar un valor | `Fin<A>` o contrato fallible existente |
| Error tipado específico de una alternativa | `Either<L,R>`, solo si el contrato necesita distinguirla |
| Lectura/escritura externa | `IO<A>` y, cuando hay ausencia, `OptionT<IO,A>` |
| Efecto que necesita capacidades del runtime | `Eff<RT,A>` |
| Secuencia de operaciones | LINQ o `Bind`, verificando sobrecargas disponibles |

No usar estas correspondencias como una taxonomía absoluta; las firmas y el significado del contrato prevalecen.

## Fuentes y ejemplos verificables

- [Dependencia efectiva de VSlices](https://github.com/vslices/framework/blob/master/src/VSlices/VSlices.csproj).
- [LanguageExt, repositorio propietario](https://github.com/louthy/language-ext) (usar documentación y código que correspondan a la versión instalada).
- [Point Algebras](https://github.com/vslices/framework/blob/master/docs/point-algebras.md), para `IO` y `OptionT<IO,A>`.
- [Feature Model](https://github.com/vslices/framework/blob/master/docs/feature-model.md), para `Eff<ALG, Response>` en VSlices.
- [Spaces Modeling](https://github.com/vslices/framework/blob/master/docs/spaces/modeling.md), para fallos de construcción y opcionalidad.

## Entrega

Producir código que compile o declarar claramente si no se pudo ejecutar la compilación; señalar el contrato funcional elegido y por qué, los casos de éxito/ausencia/fallo pertinentes y cualquier diferencia entre ejemplos consultados y APIs efectivamente disponibles.

# Alternativas semánticas y uniones discriminadas en Spaces

## Pregunta central

Cuando una propiedad conceptual admite **alternativas de significado mutuamente excluyentes**, identificar cada caso y sus invariantes antes de escoger representación. Ejemplo: una inscripción puede corresponder a una persona con nombre de pila o a una organización identificada mediante razón social. El hecho de que ambas variantes puedan representarse técnicamente como texto **no las convierte en el mismo concepto**.

Distinguir:
- **Ausencia**: `Option<A>` cuando el valor puede no existir.
- **Alternativa exclusiva**: unión discriminada cuando existe **exactamente una** entre variantes semánticamente distintas.
- **Conjunción**: estructura compuesta cuando deben coexistir varias propiedades.
- **Error de transformación**: `Fin<A>`, que no sustituye la distinción entre variantes válidas.
- **Evolución**: las invariantes que prohíben o permiten cambiar de variante se determinan por el dominio, no por la forma de la unión.

## Procedimiento

1. Determinar qué variantes admite el dominio, qué evidencia identifica a cada una y qué restricciones son propias.
2. Modelar los valores básicos como Spaces diferenciados cuando tienen significado propio, aun si comparten backing `string`.
3. Representar las variantes nominales del mismo concepto mediante una abstracción común y subtipos/casos concretos (por ejemplo `Report` con `DraftReport` y `CompletedReport`). Conservar la identidad compartida y estudiar explícitamente si dos variantes con el mismo identificador son iguales o no. `Either<A,B>` puede representar dos alternativas válidas cuando tenga significado propio, pero no envolver automáticamente una jerarquía nominal en `Either<Draft,Completed>`: duplicaría la discriminación. Comprobar las restricciones de exhaustividad y construcción del lenguaje elegido.
4. Impedir estados ambiguos o imposibles como dos propiedades opcionales independientes `Option<GivenName>` y `Option<LegalName>` cuando el contrato exige exactamente una: esa forma permitiría **ninguna** o **ambas**.
5. No resolver una variante a partir de heurísticas del texto (mayúsculas, espacios, longitud, palabras como «SpA»); exigir evidencia discriminadora en la frontera de construcción si no hay mecanismo fiable. No inventar requisitos para nombres, apellidos, registro comercial, documentos o personería.
6. Preservar invariantes de cada variante mediante su Space propietario, y de la unión mediante una composición que no acepte combinaciones imposibles.
7. Probar ambas ramas, los intentos de ambigüedad y las transformaciones pertinentes. Cuando el discriminador o las reglas de transición entre variantes no estén claros, incluirlos como información no especificada en el informe.

## Variante nominal con identidad compartida (conceptual)

```csharp
abstract record Report(ReportId Id);
sealed record DraftReport(ReportId Id) : Report(Id);
sealed record CompletedReport(ReportId Id, ReportResult Result) : Report(Id);
```

El ejemplo no declara todos los contratos de VSlices ni pretende compilar aislado: sus transformaciones, constructores, igualdad y garantías deben resolverse contra el Framework actual. Un `DraftReport` y un `CompletedReport` son dos variantes de `Report`, sin `Either<DraftReport,CompletedReport>` adicional.

## Ejemplo conceptual (sin imponer contratos)

```text
ParticipantDesignation
  = Personal(GivenName)
  | Organization(LegalName)
```

La forma es ilustrativa. `GivenName` y `LegalName` representan conceptos distintos; `ParticipantDesignation` representa exactamente uno. No inferir que el nombre de pila equivale al nombre completo o que necesariamente se requieren apellidos. Si ambos nombres comparten el mismo tipo primitivo, `Either<string,string>` pierde la distinción nominal pretendida.

## Errores a evitar

- `string ParticipantName` sin evidencia del tipo de participante cuando las variantes son semánticamente relevantes.
- `Option<GivenName>` junto a `Option<LegalName>` sin una regla de exclusión y existencia demostrable.
- `Either<Error, GivenName>` para expresar persona/organización: confunde un caso válido con fallo.
- Crear subtipos de participante, identificaciones tributarias o registros externos que no estén exigidos por el dominio.
- Tratar la ausencia de apellidos, el formato legal o la posibilidad de cambiar de variante como reglas resueltas sin evidencia.

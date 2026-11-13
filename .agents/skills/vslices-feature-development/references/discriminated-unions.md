# Consumo de variantes nominales en Features

Una Feature puede recibir o producir un Space que represente variantes nominales de un mismo concepto. Por ejemplo, `DraftEvaluationReport` y `CompletedEvaluationReport` pueden ser subtipos de `EvaluationReport`, con características propias y una identidad compartida.

La Feature debe distinguir las variantes de forma explícita —por pattern matching o mecanismo exhaustivo disponible— para ejecutar la operación correspondiente. No envolver una jerarquía nominal ya discriminada en `Either<Draft,Completed>` sin otra distinción semántica: duplicaría la alternativa.

La propiedad del discriminador, los estados representables, su igualdad y las transiciones admisibles pertenecen al Space correspondiente. La Feature no debe inventar estados ni saltarse la construcción/transformación del dominio. Para variantes que implican efectos distintos, separar primero el significado de cada rama y luego componer los efectos necesarios.

La exhaustividad real depende de la representación y del compilador; una clase base abstracta por sí sola no garantiza que C# obligue a tratar todos los subtipos. Verificar comportamiento y añadir pruebas por variante; no prometer garantías estáticas inexistentes.

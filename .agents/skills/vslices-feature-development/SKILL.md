---
name: vslices-feature-development
description: >-
  Modelar, implementar y revisar Features en VSlices Work (.NET), sus módulos,
  algebras de capacidades, Request/Response y efectos Eff, IO, FinT y transformadores.
  Usar cuando una tarea cree o cambie operaciones de aplicación, validación contextual
  o composición de Features, respetando las Laws y los contratos actuales del Framework.
---

# Desarrollo de Features en VSlices

## Alcance

Una Feature es una operación ejecutable de **Work**. Interpreta un Request, utiliza capacidades representadas por un Algebra y produce un `Eff<ALG, Response>`. No reemplaza el modelado de Spaces ni la realización de efectos que corresponde a Grounding.

Consultar el [contrato actual de Feature](https://github.com/vslices/framework/blob/master/src/VSlices.Work/Feature.cs) antes de implementar. No inferir APIs de versiones históricas. Usar `language-ext-development` para firmas concretas de LanguageExt v5 y `vslices-spaces-development` para modelar invariantes propias de Spaces.

## Procedimiento

1. **Delimitar la operación.** Identificar el módulo propietario, su Request, el resultado reconocible y la evidencia necesaria. No mover invariantes del dominio a la Feature; distinguir validez del Space de admisibilidad contextual.
2. **Identificar capacidades mínimas.** Cada Feature declara un `public interface Algebra` anidado que expone únicamente sus capacidades ejecutables requeridas; el Algebra concreto del módulo implementa las interfaces de sus Features. No utilizar directamente el Algebra completo del módulo como `ALG` de cada Feature. Cuando la interfaz `Algebra` no declare miembros propios y solo herede capacidades, usar la declaración compacta de C# 12+ con `;` en lugar del cuerpo vacío `{ }` (por ejemplo, `public interface Algebra : HasPointReader<Item, ItemId>, HasPointWriter<Item>;`). Si declara miembros, conservar el cuerpo normal. Es una convención de estilo de autoría, no una Law estructural nueva. Usar [módulos y algebras](references/modules-and-algebras.md) y [Point Algebras](https://github.com/vslices/framework/blob/master/docs/point-algebras.md). No introducir Repository, transacciones ni otras garantías por analogía.
3. **Elegir validaciones.** Para admisibilidad contextual pura evaluar `Validatable<CTX,SPACE>`; para admisibilidad con efectos `ValidatableM<CTX,M,SPACE>`; para transformación con efectos `TransformableM<CTX,M,FROM,TO>`. Examinar sus contratos reales y [validación de Features](references/feature-validation.md). `Transformable` puro es relevante cuando una transformación de dominio es propiedad de su Space, no una rutina de Work.
4. **Implementar contrato nominal.** Declarar `Feature<SELF,SELF.Algebra,SELF.Request,SELF.Response>` con `static Eff<ALG,Response> Get(Request request)`. `Request` público y construible por el consumidor; `Response` público con constructor privado, miembros de solo lectura y factoría privada `NewResponse` mediante `UnsafeAccessor` según las Laws vigentes.
5. **Componer ejecución.** Mantener los efectos en `IO`/`Eff` y transformadores cuando correspondan. Usar los hechos obtenidos por capacidades antes de ejecutar reglas puras del dominio; [composición de efectos](references/effect-composition.md). Para alternativas nominales, usar [uniones discriminadas](references/discriminated-unions.md).
6. **Verificar.** Compilar el consumidor cuando se modifique C#, ejecutar `FeatureModelingLaws.InspectDefinitions(assembly, featuresNamespace)` o `Inspect(type)` y revisar FM001–FM011 (FM007 retirado). FM011 exige interfaz `Algebra` pública y anidada, vinculada exactamente a `ALG`; revisar por separado que sus capacidades sean realmente mínimas, porque la reflexión no demuestra cuáles usa el cuerpo de `Get`. Probar éxito, fallos esperados, ausencia, propagación, efectos realmente realizados y composición entre módulos. Las Laws son estructurales: no prueban ejecución correcta ni atomicidad.
7. **Informar.** Entregar código y [reporte de modelado de Features](references/feature-modeling-report.md), separando evidencia, decisiones, vacíos y riesgos.

## Reglas de responsabilidad

- Cada módulo tiene su carpeta con su Algebra concreto y `Features/`; su significado, no un namespace accidental, define la pertenencia. Cada Feature posee su contrato `Algebra` de capacidades mínimas. El tipo concreto del módulo implementa esos contratos y puede ser pasado a `Run`; no se requiere `in ALG` en el trait `Feature`.
- **Feature:** propone una operación, valida uso contextual y coordina capacidades; **Space:** posee significado, invariantes y evolución admisible; **Grounding:** implementa las capacidades que contactan el mundo.
- Una función pura, por ejemplo elegibilidad de transición `Func<Facts,Error>`, puede permanecer como característica de un Space. La Feature obtiene los hechos y decide la ejecución.
- Un `Eff<ALG,Response>` no demuestra que un efecto se completó. Responder de manera exitosa no debe encubrir un rechazo esperado.
- No renombrar automáticamente `Capacities`: su nomenclatura es materia de revisión, no decisión adoptada.
- Esta cobertura es inicial y se revisará según observaciones de agentes; no convertir cada realización particular en una regla global.

## Referencias

- [Feature Model](https://github.com/vslices/framework/blob/master/docs/feature-model.md)
- [Feature Modeling Laws](https://github.com/vslices/framework/blob/master/docs/work/feature-modeling.md)
- [Validatable](https://github.com/vslices/framework/blob/master/src/VSlices.Spaces/Traits/Validatable.cs), [ValidatableM](https://github.com/vslices/framework/blob/master/src/VSlices.Spaces/Traits/ValidatableM.cs), [TransformableM](https://github.com/vslices/framework/blob/master/src/VSlices.Spaces/Traits/TransformableM.cs)
- [Módulos y algebras](references/modules-and-algebras.md)
- [Validación contextual](references/feature-validation.md)
- [Composición de efectos](references/effect-composition.md)
- [Uniones discriminadas](references/discriminated-unions.md)
- [Reporte](references/feature-modeling-report.md)

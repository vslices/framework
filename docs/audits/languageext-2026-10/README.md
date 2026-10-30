# LanguageExt usage census

**Method:** exhaustive lexical scan of tracked-like source/config files; NOT Roslyn-bound symbol attribution.
Files and line numbers refer to the pinned checkout commit.

- Files scanned: **209**
- Candidate/reference location rows: **1637**
- Files with matches: **108**
- Files with explicit LanguageExt imports/references: **58**

## Classification and caveats

- **Certain**: textual namespace reference, project package declaration, MSBuild global import.
- **Candidate**: a type/trait token, Prelude invocation or extension-style method. Some are VSlices-owned or BCL; resolve with Roslyn before assigning origin.
- Implicit and transitive usages are not fully attributable without compilation and symbol binding.
- String literals, comments and documentation embedded in C# are included in the lexical census and need filtering.

## Candidate symbol frequencies

| Symbol | Occurrences |
|---|---:|
| `K` | 325 |
| `IO` | 158 |
| `Fin` | 144 |
| `Bind` | 106 |
| `Eff` | 92 |
| `Map` | 52 |
| `Unit` | 51 |
| `Error` | 49 |
| `RunAsync` | 48 |
| `FinT` | 45 |
| `Match` | 40 |
| `Option` | 35 |
| `Run` | 33 |
| `Success` | 26 |
| `pure` | 24 |
| `Lift` | 24 |
| `State` | 23 |
| `As` | 22 |
| `Either` | 21 |
| `lift` | 20 |
| `Reader` | 18 |
| `Pure` | 16 |
| `Writer` | 14 |
| `Monad` | 13 |
| `Right` | 10 |
| `Prelude` | 10 |
| `OptionT` | 10 |
| `Some` | 9 |
| `ThrowIfFail` | 8 |
| `Left` | 6 |
| `IfNone` | 6 |
| `Applicative` | 6 |
| `fail` | 5 |
| `RunIO` | 5 |
| `Readable` | 5 |
| `ArrowApply` | 5 |
| `Fallible` | 4 |
| `Kleisli` | 4 |
| `Select` | 3 |
| `Alternative` | 3 |
| `Final` | 3 |
| `Functor` | 3 |
| `MonadUnliftIO` | 3 |
| `Seq` | 3 |
| `Flatten` | 2 |
| `Choose` | 2 |
| `MonadIO` | 2 |
| `liftIO` | 2 |
| `SemigroupK` | 2 |
| `MonoidK` | 2 |
| `FailSpan` | 2 |

## File index

| File | Lines with matches | Raw matches |
|---|---:|---:|
| [`.github/workflows/languageext-usage-audit.yml`](../../../../.github/workflows/languageext-usage-audit.yml) | 2 | 2 |
| [`examples/SampleBFF/SampleBFF.Tests/CrossServiceProcessTests.cs`](../../../../examples/SampleBFF/SampleBFF.Tests/CrossServiceProcessTests.cs) | 14 | 14 |
| [`examples/SampleBFF/SampleBFF/AttachFileToTodo.cs`](../../../../examples/SampleBFF/SampleBFF/AttachFileToTodo.cs) | 18 | 23 |
| [`examples/SampleFileRepo/SampleFileRepo/AddFile.cs`](../../../../examples/SampleFileRepo/SampleFileRepo/AddFile.cs) | 3 | 3 |
| [`examples/SampleFileRepo/SampleFileRepo/FileAlgebra.cs`](../../../../examples/SampleFileRepo/SampleFileRepo/FileAlgebra.cs) | 4 | 5 |
| [`examples/SampleFileRepo/SampleFileRepo/GetFile.cs`](../../../../examples/SampleFileRepo/SampleFileRepo/GetFile.cs) | 4 | 6 |
| [`examples/SampleFileRepo/SampleFileRepo/InMemoryFileWork.cs`](../../../../examples/SampleFileRepo/SampleFileRepo/InMemoryFileWork.cs) | 13 | 23 |
| [`examples/SampleWorkflow/SampleWorkflow.Api/Program.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Api/Program.cs) | 15 | 15 |
| [`examples/SampleWorkflow/SampleWorkflow.Grounding/InMemoryTodoWork.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Grounding/InMemoryTodoWork.cs) | 19 | 33 |
| [`examples/SampleWorkflow/SampleWorkflow.Process.Tests/ProcessHoistTests.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Process.Tests/ProcessHoistTests.cs) | 4 | 4 |
| [`examples/SampleWorkflow/SampleWorkflow.Process/CreateAndGetTodo.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Process/CreateAndGetTodo.cs) | 13 | 24 |
| [`examples/SampleWorkflow/SampleWorkflow.Spaces/ResourceReference.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Spaces/ResourceReference.cs) | 2 | 3 |
| [`examples/SampleWorkflow/SampleWorkflow.Spaces/Todo.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Spaces/Todo.cs) | 9 | 12 |
| [`examples/SampleWorkflow/SampleWorkflow.Spaces/TodoDetail.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Spaces/TodoDetail.cs) | 2 | 3 |
| [`examples/SampleWorkflow/SampleWorkflow.Spaces/TodoId.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Spaces/TodoId.cs) | 2 | 3 |
| [`examples/SampleWorkflow/SampleWorkflow.Work/AddAttachmentReference.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Work/AddAttachmentReference.cs) | 20 | 40 |
| [`examples/SampleWorkflow/SampleWorkflow.Work/CreateTodo.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Work/CreateTodo.cs) | 18 | 34 |
| [`examples/SampleWorkflow/SampleWorkflow.Work/DeleteTodo.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Work/DeleteTodo.cs) | 8 | 14 |
| [`examples/SampleWorkflow/SampleWorkflow.Work/GetTodo.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Work/GetTodo.cs) | 4 | 6 |
| [`examples/SampleWorkflow/SampleWorkflow.Work/TodoAlgebra.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Work/TodoAlgebra.cs) | 4 | 5 |
| [`examples/SampleWorkflow/SampleWorkflow.Work/UpdateTodo.cs`](../../../../examples/SampleWorkflow/SampleWorkflow.Work/UpdateTodo.cs) | 17 | 33 |
| [`examples/VSlices.Spaces.Modeling/CoordinateProductProbe.cs`](../../../../examples/VSlices.Spaces.Modeling/CoordinateProductProbe.cs) | 1 | 1 |
| [`examples/VSlices.Spaces.Modeling/InvalidUsage.cs`](../../../../examples/VSlices.Spaces.Modeling/InvalidUsage.cs) | 6 | 6 |
| [`examples/VSlices.Spaces.Modeling/Location.cs`](../../../../examples/VSlices.Spaces.Modeling/Location.cs) | 12 | 18 |
| [`examples/VSlices.Spaces.Modeling/Quantities/Mass.cs`](../../../../examples/VSlices.Spaces.Modeling/Quantities/Mass.cs) | 2 | 3 |
| [`examples/VSlices.Spaces.Modeling/Quantities/Quantity.cs`](../../../../examples/VSlices.Spaces.Modeling/Quantities/Quantity.cs) | 1 | 1 |
| [`examples/VSlices.Spaces.Modeling/QuantityFamilies/QuantityFamily.cs`](../../../../examples/VSlices.Spaces.Modeling/QuantityFamilies/QuantityFamily.cs) | 11 | 11 |
| [`examples/VSlices.Spaces.Modeling/QuantityFamilies/SelfMass.cs`](../../../../examples/VSlices.Spaces.Modeling/QuantityFamilies/SelfMass.cs) | 6 | 6 |
| [`examples/VSlices.Spaces.Modeling/QuantityFamilies/StaticSelfMass.cs`](../../../../examples/VSlices.Spaces.Modeling/QuantityFamilies/StaticSelfMass.cs) | 6 | 6 |
| [`examples/VSlices.Spaces.Modeling/Usage.cs`](../../../../examples/VSlices.Spaces.Modeling/Usage.cs) | 9 | 14 |
| [`src/VSlices.Grounding.EntityFrameworkCore/EntityFrameworkPointIO.cs`](../../../../src/VSlices.Grounding.EntityFrameworkCore/EntityFrameworkPointIO.cs) | 15 | 25 |
| [`src/VSlices.Grounding/SystemTimeIO.cs`](../../../../src/VSlices.Grounding/SystemTimeIO.cs) | 8 | 15 |
| [`src/VSlices.Spaces.Analyzers/TransformAdapterAnalyzer.cs`](../../../../src/VSlices.Spaces.Analyzers/TransformAdapterAnalyzer.cs) | 3 | 3 |
| [`src/VSlices.Spaces.Products/Role.cs`](../../../../src/VSlices.Spaces.Products/Role.cs) | 4 | 4 |
| [`src/VSlices.Spaces/Finance/ExchangeRate.cs`](../../../../src/VSlices.Spaces/Finance/ExchangeRate.cs) | 5 | 7 |
| [`src/VSlices.Spaces/MaintainedSpace.cs`](../../../../src/VSlices.Spaces/MaintainedSpace.cs) | 2 | 2 |
| [`src/VSlices.Spaces/Quantities/Duration.cs`](../../../../src/VSlices.Spaces/Quantities/Duration.cs) | 1 | 1 |
| [`src/VSlices.Spaces/Quantities/Length.cs`](../../../../src/VSlices.Spaces/Quantities/Length.cs) | 1 | 1 |
| [`src/VSlices.Spaces/Quantities/Mass.cs`](../../../../src/VSlices.Spaces/Quantities/Mass.cs) | 1 | 1 |
| [`src/VSlices.Spaces/Quantities/Temperature.cs`](../../../../src/VSlices.Spaces/Quantities/Temperature.cs) | 5 | 8 |
| [`src/VSlices.Spaces/Traits/Evolvable.cs`](../../../../src/VSlices.Spaces/Traits/Evolvable.cs) | 5 | 9 |
| [`src/VSlices.Spaces/Traits/TransformAdapter.cs`](../../../../src/VSlices.Spaces/Traits/TransformAdapter.cs) | 10 | 18 |
| [`src/VSlices.Spaces/Traits/Transformable.cs`](../../../../src/VSlices.Spaces/Traits/Transformable.cs) | 4 | 7 |
| [`src/VSlices.Spaces/Traits/TransformableM.cs`](../../../../src/VSlices.Spaces/Traits/TransformableM.cs) | 8 | 11 |
| [`src/VSlices.Spaces/Traits/Validatable.cs`](../../../../src/VSlices.Spaces/Traits/Validatable.cs) | 3 | 5 |
| [`src/VSlices.Spaces/Traits/ValidatableM.cs`](../../../../src/VSlices.Spaces/Traits/ValidatableM.cs) | 5 | 7 |
| [`src/VSlices.Spaces/VSlices.Spaces.csproj`](../../../../src/VSlices.Spaces/VSlices.Spaces.csproj) | 4 | 8 |
| [`src/VSlices.Views.Abstract/Forms/vTextInput.cs`](../../../../src/VSlices.Views.Abstract/Forms/vTextInput.cs) | 2 | 3 |
| [`src/VSlices.Views.Abstract/VSlices.Views.Abstract.csproj`](../../../../src/VSlices.Views.Abstract/VSlices.Views.Abstract.csproj) | 2 | 4 |
| [`src/VSlices.Work.Services/Traits/ServiceFeature.cs`](../../../../src/VSlices.Work.Services/Traits/ServiceFeature.cs) | 2 | 2 |
| [`src/VSlices.Work/Algebra/PointReader.cs`](../../../../src/VSlices.Work/Algebra/PointReader.cs) | 8 | 11 |
| [`src/VSlices.Work/Algebra/PointRemover.cs`](../../../../src/VSlices.Work/Algebra/PointRemover.cs) | 1 | 3 |
| [`src/VSlices.Work/Algebra/PointWriter.cs`](../../../../src/VSlices.Work/Algebra/PointWriter.cs) | 1 | 3 |
| [`src/VSlices.Work/Feature.cs`](../../../../src/VSlices.Work/Feature.cs) | 1 | 1 |
| [`src/VSlices.Work/TemporalIO.cs`](../../../../src/VSlices.Work/TemporalIO.cs) | 3 | 8 |
| [`src/VSlices.Work/VSlices.Work.csproj`](../../../../src/VSlices.Work/VSlices.Work.csproj) | 5 | 10 |
| [`src/VSlices/Literals/Charsets.cs`](../../../../src/VSlices/Literals/Charsets.cs) | 1 | 1 |
| [`src/VSlices/Monads/Eff/Syntax/Monad/Eff2.Monad.Fin.cs`](../../../../src/VSlices/Monads/Eff/Syntax/Monad/Eff2.Monad.Fin.cs) | 38 | 67 |
| [`src/VSlices/Monads/Eff/Syntax/Monad/Eff2.Monad.Flow.cs`](../../../../src/VSlices/Monads/Eff/Syntax/Monad/Eff2.Monad.Flow.cs) | 15 | 24 |
| [`src/VSlices/Monads/Fin/Syntax/Monad/Fin.Monad.Eff2.cs`](../../../../src/VSlices/Monads/Fin/Syntax/Monad/Fin.Monad.Eff2.cs) | 31 | 51 |
| [`src/VSlices/Monads/Flow/Flow.Definition.cs`](../../../../src/VSlices/Monads/Flow/Flow.Definition.cs) | 15 | 20 |
| [`src/VSlices/Monads/Flow/Flow.Dependencies.cs`](../../../../src/VSlices/Monads/Flow/Flow.Dependencies.cs) | 24 | 32 |
| [`src/VSlices/Monads/Flow/Syntax/Flow.Dependencies.cs`](../../../../src/VSlices/Monads/Flow/Syntax/Flow.Dependencies.cs) | 10 | 12 |
| [`src/VSlices/Monads/Flow/Syntax/Flow.DependencyPrelude.cs`](../../../../src/VSlices/Monads/Flow/Syntax/Flow.DependencyPrelude.cs) | 2 | 4 |
| [`src/VSlices/Monads/Flow/Syntax/Flow.Final.cs`](../../../../src/VSlices/Monads/Flow/Syntax/Flow.Final.cs) | 5 | 5 |
| [`src/VSlices/Monads/Flow/Syntax/Flow.Functor.cs`](../../../../src/VSlices/Monads/Flow/Syntax/Flow.Functor.cs) | 16 | 16 |
| [`src/VSlices/Monads/Flow/Syntax/Flow.Kind.cs`](../../../../src/VSlices/Monads/Flow/Syntax/Flow.Kind.cs) | 10 | 13 |
| [`src/VSlices/Monads/Flow/Syntax/Monad/Flow.Monad.Eff.cs`](../../../../src/VSlices/Monads/Flow/Syntax/Monad/Flow.Monad.Eff.cs) | 29 | 38 |
| [`src/VSlices/Monads/Flow/Syntax/Monad/Flow.Monad.Eff2.cs`](../../../../src/VSlices/Monads/Flow/Syntax/Monad/Flow.Monad.Eff2.cs) | 29 | 38 |
| [`src/VSlices/Monads/Flow/Syntax/Monad/Flow.Monad.Fin.cs`](../../../../src/VSlices/Monads/Flow/Syntax/Monad/Flow.Monad.Fin.cs) | 29 | 47 |
| [`src/VSlices/Monads/Flow/Syntax/Monad/Flow.Monad.FinT+Eff.cs`](../../../../src/VSlices/Monads/Flow/Syntax/Monad/Flow.Monad.FinT+Eff.cs) | 31 | 88 |
| [`src/VSlices/Monads/Flow/Syntax/Monad/Flow.Monad.IO.cs`](../../../../src/VSlices/Monads/Flow/Syntax/Monad/Flow.Monad.IO.cs) | 22 | 32 |
| [`src/VSlices/Monads/Flow/Syntax/Monad/Flow.Monad.cs`](../../../../src/VSlices/Monads/Flow/Syntax/Monad/Flow.Monad.cs) | 18 | 21 |
| [`src/VSlices/Monads/Flow/Trait/Flow.Alternative+Choice.cs`](../../../../src/VSlices/Monads/Flow/Trait/Flow.Alternative+Choice.cs) | 7 | 13 |
| [`src/VSlices/Monads/Flow/Trait/Flow.Fallible+Final+Readable.cs`](../../../../src/VSlices/Monads/Flow/Trait/Flow.Fallible+Final+Readable.cs) | 30 | 45 |
| [`src/VSlices/Monads/Flow/Trait/Flow.Functor+Applicative.cs`](../../../../src/VSlices/Monads/Flow/Trait/Flow.Functor+Applicative.cs) | 26 | 35 |
| [`src/VSlices/Monads/Flow/Trait/Flow.Monad+MonadIO+MonadUnliftIO.cs`](../../../../src/VSlices/Monads/Flow/Trait/Flow.Monad+MonadIO+MonadUnliftIO.cs) | 33 | 59 |
| [`src/VSlices/Monads/Flow/Trait/Flow.MonoidK+SemigroupK.cs`](../../../../src/VSlices/Monads/Flow/Trait/Flow.MonoidK+SemigroupK.cs) | 7 | 10 |
| [`src/VSlices/Monads/Flow/Trait/Flow.Traits.cs`](../../../../src/VSlices/Monads/Flow/Trait/Flow.Traits.cs) | 6 | 7 |
| [`src/VSlices/Traits/Arrow/Arrow.Laws.cs`](../../../../src/VSlices/Traits/Arrow/Arrow.Laws.cs) | 25 | 33 |
| [`src/VSlices/Traits/Arrow/Arrow.Module.cs`](../../../../src/VSlices/Traits/Arrow/Arrow.Module.cs) | 16 | 19 |
| [`src/VSlices/Traits/Arrow/Arrow.Trait.cs`](../../../../src/VSlices/Traits/Arrow/Arrow.Trait.cs) | 20 | 21 |
| [`src/VSlices/Traits/ArrowApply/ArrowApply.Module.cs`](../../../../src/VSlices/Traits/ArrowApply/ArrowApply.Module.cs) | 6 | 9 |
| [`src/VSlices/Traits/ArrowApply/ArrowApply.Trait.cs`](../../../../src/VSlices/Traits/ArrowApply/ArrowApply.Trait.cs) | 7 | 9 |
| [`src/VSlices/Traits/Category/Category.Laws.cs`](../../../../src/VSlices/Traits/Category/Category.Laws.cs) | 8 | 11 |
| [`src/VSlices/Traits/Category/Category.Module.cs`](../../../../src/VSlices/Traits/Category/Category.Module.cs) | 26 | 26 |
| [`src/VSlices/Traits/Category/Category.Trait.cs`](../../../../src/VSlices/Traits/Category/Category.Trait.cs) | 26 | 26 |
| [`src/VSlices/Traits/Kleisli/Kleisli.Module.cs`](../../../../src/VSlices/Traits/Kleisli/Kleisli.Module.cs) | 4 | 5 |
| [`src/VSlices/Traits/Kleisli/Kleisli.Trait.cs`](../../../../src/VSlices/Traits/Kleisli/Kleisli.Trait.cs) | 5 | 5 |
| [`src/VSlices/VSlices.csproj`](../../../../src/VSlices/VSlices.csproj) | 8 | 16 |
| [`tests/VSlices.Grounding.EntityFrameworkCore.Tests/EntityFrameworkPointIOTests.cs`](../../../../tests/VSlices.Grounding.EntityFrameworkCore.Tests/EntityFrameworkPointIOTests.cs) | 30 | 32 |
| [`tests/VSlices.Grounding.Tests/SystemTimeIOTests.cs`](../../../../tests/VSlices.Grounding.Tests/SystemTimeIOTests.cs) | 3 | 3 |
| [`tests/VSlices.Spaces.Analyzers.Tests/TransformAdapterAnalyzerTests.cs`](../../../../tests/VSlices.Spaces.Analyzers.Tests/TransformAdapterAnalyzerTests.cs) | 1 | 1 |
| [`tests/VSlices.Spaces.Modeling.Tests/QuantityFamilyModelingTests.cs`](../../../../tests/VSlices.Spaces.Modeling.Tests/QuantityFamilyModelingTests.cs) | 1 | 1 |
| [`tests/VSlices.Spaces.Modeling.Tests/QuantityModelingTests.cs`](../../../../tests/VSlices.Spaces.Modeling.Tests/QuantityModelingTests.cs) | 5 | 7 |
| [`tests/VSlices.Spaces.Modeling.Tests/SelfMassModelingTests.cs`](../../../../tests/VSlices.Spaces.Modeling.Tests/SelfMassModelingTests.cs) | 1 | 1 |
| [`tests/VSlices.Spaces.Modeling.Tests/SpaceModelingTests.cs`](../../../../tests/VSlices.Spaces.Modeling.Tests/SpaceModelingTests.cs) | 13 | 15 |
| [`tests/VSlices.Spaces.Modeling.Tests/StaticSelfMassModelingTests.cs`](../../../../tests/VSlices.Spaces.Modeling.Tests/StaticSelfMassModelingTests.cs) | 1 | 1 |
| [`tests/VSlices.Spaces.Tests/MoneyTests.cs`](../../../../tests/VSlices.Spaces.Tests/MoneyTests.cs) | 11 | 13 |
| [`tests/VSlices.Spaces.Tests/TemperatureTests.cs`](../../../../tests/VSlices.Spaces.Tests/TemperatureTests.cs) | 9 | 11 |
| [`tests/VSlices.TransformAdapter.AotProbe/Program.cs`](../../../../tests/VSlices.TransformAdapter.AotProbe/Program.cs) | 7 | 11 |
| [`tests/VSlices.Views.Abstract.Tests/TransformAdapterUiTests.cs`](../../../../tests/VSlices.Views.Abstract.Tests/TransformAdapterUiTests.cs) | 8 | 12 |
| [`tests/VSlices.Views.Razor.AdapterProbe/EmailAddress.cs`](../../../../tests/VSlices.Views.Razor.AdapterProbe/EmailAddress.cs) | 3 | 5 |
| [`tests/VSlices.Work.Algebra.Tests/AlgebraMixArityTests.cs`](../../../../tests/VSlices.Work.Algebra.Tests/AlgebraMixArityTests.cs) | 11 | 13 |
| [`tests/VSlices.Work.Algebra.Tests/FlowDependencyCompositionTests.cs`](../../../../tests/VSlices.Work.Algebra.Tests/FlowDependencyCompositionTests.cs) | 15 | 22 |
| [`tests/VSlices.Work.Algebra.Tests/FlowTransformTests.cs`](../../../../tests/VSlices.Work.Algebra.Tests/FlowTransformTests.cs) | 12 | 13 |
| [`tests/VSlices.Work.Algebra.Tests/PointAlgebraTests.cs`](../../../../tests/VSlices.Work.Algebra.Tests/PointAlgebraTests.cs) | 34 | 49 |
| [`tests/VSlices.Work.Algebra.Tests/TemporalAlgebraTests.cs`](../../../../tests/VSlices.Work.Algebra.Tests/TemporalAlgebraTests.cs) | 13 | 21 |

## Outputs

- `all-locations.csv`: file, line, column, symbol, classification, source snippet.
- `packages.csv`: dependency declarations.
- `msbuild-imports.csv`: imported global namespace entries.
- `project-references.csv`: graph edges.
- `files-scanned.csv`: coverage evidence.
- `summary.json`: counters and collisions.

Do not treat matching a token such as `Map`, `Error`, `Fin` or `Monad` as proof of LanguageExt ownership.
Separate reproduced implementation code from conceptual inspiration for licensing/attribution.

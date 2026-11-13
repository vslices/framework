# Módulos, algebras y capacidades ejecutables

Cada módulo de Work posee su carpeta y un Algebra explícito con sus Features:

```text
Tickets.Work/
├── Tickets/
│   ├── TicketAlgebra.cs
│   └── Features/
├── Evaluations/
│   ├── EvaluationAlgebra.cs
│   └── Features/
├── Approvals/
│   ├── ApprovalAlgebra.cs
│   └── Features/
└── Supervisions/
    ├── SupervisionAlgebra.cs
    └── Features/
```

Los nombres son conceptuales; examinar la estructura existente y no mover archivos sin presión verificable.

El Algebra expone las **capacidades ejecutables mínimas** requeridas por las Features. [Point Algebras](https://github.com/vslices/framework/blob/master/docs/point-algebras.md) define `PointReader<POINT,ID>` (lectura opcional `OptionT<IO,POINT>`), `PointWriter<POINT>` y `PointRemover<POINT,ID>`. Estos contratos no prometen transacciones, tracking, aislamiento, durabilidad ni Repository.

Una Feature obtiene un `Eff<ALG,Response>`; Grounding proporciona una realización del Algebra. Para composición entre módulos puede emplearse `AlgebraMix<A,B>`, proyectando la capacidad del hijo mediante `Child.Get(request).RunIO(mix.A)`. Una mezcla no concede garantías de rollback o compensación. Consultar firmas actuales antes de implementar.

El nombre `Capacities` está sujeto a examen semántico. No renombrar tipos, namespaces o carpetas como parte automática de esta Skill.

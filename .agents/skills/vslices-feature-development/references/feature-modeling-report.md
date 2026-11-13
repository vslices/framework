# Informe mínimo de modelado de Features

Al finalizar una realización, registrar:

| Categoría | Pregunta de revisión |
| --- | --- |
| Autoridad | ¿Qué módulo y Feature poseen la operación? ¿El Space conserva sus invariantes y Grounding sus efectos? |
| Contrato | ¿`Feature<SELF,ALG,Request,Response>` coincide con los tipos nominales anidados? |
| Validación | ¿Dónde se utilizan `Validatable`, `ValidatableM` y `TransformableM`? ¿La regla requiere evidencia externa? |
| Algebra | ¿Cada capacidad está justificada? ¿Se infirieron garantías no presentes en los contratos? |
| Composición | ¿Se preserva la ausencia, el fracaso y la ejecución del efecto? ¿Se proyectan correctamente los algebras hijos? |
| Alternativas | ¿Todas las variantes nominales relevantes son tratadas sin duplicar la discriminación? |
| Evidencia | ¿Compiló, pasó FM001–FM010 y se probaron éxito, fallos y efectos reales? |

Distinguir **incoherencia verificada**, **información no especificada**, **decisión provisional** y **límite de verificación**. Si no hay hallazgos, declarar que no se identificaron incoherencias verificadas; no fabricar observaciones. FM007 está retirado.

Las Laws estructurales inspeccionan el contrato visible; no demuestran que una operación completó sus efectos, que el resultado es correcto o que hay transacciones. No registrar como ejecutadas verificaciones que no se realizaron.

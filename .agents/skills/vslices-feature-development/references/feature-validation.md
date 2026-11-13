# Validación contextual y transformación con efectos

Los contratos actuales del Framework distinguen las siguientes operaciones:

| Contrato | Firma abstracta | Propósito |
| --- | --- | --- |
| `Validatable<CTX,SPACE>` | `static Fin<SPACE> Validate(SPACE value)` | Admisibilidad contextual pura |
| `ValidatableM<CTX,M,SPACE>` | `static FinT<M,SPACE> Validate(SPACE value)` | Admisibilidad contextual con efectos |
| `TransformableM<CTX,M,FROM,TO>` | `static FinT<M,TO> Transform(FROM source)` | Transformación semántica con efectos |
| `Transformable<CTX,FROM,TO>` | `static Fin<TO> Transform(FROM source)` | Transformación pura de autoridad semántica |

En los dos traits con `M`, el parámetro satisface `where M : Monad<M>`. No asumir retornos `Fin<...>` donde el contrato exige `FinT<M,...>` ni inventar conversiones.

La distinción clave: **válido como Space** no implica **admisible en una operación concreta**. Por ejemplo, un Ticket puede ser un punto semánticamente válido y no estar disponible para aprobación por estado o permisos comprobados. Cuando esa comprobación necesita capacidades externas, corresponde examinar `ValidatableM` en lugar de introducir IO en el Space.

La Feature puede componer la validación con las capacidades que obtienen evidencia. No duplicar invariantes del Space ni confundir fallas del efecto con ausencia válida. Los helpers existentes `Validatable.Check`, `ValidatableM.Check` y `TransformableM.Transform` son puntos de invocación genérica; verificar sus parámetros al emplearlos.

La Skill enseña estos contratos y su selección, no supone que todas las Features deban implementar los cuatro traits. Probar casos admitidos, rechazados, propagación de fallos y adquisición de evidencia antes de declarar el flujo correcto.

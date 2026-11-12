# Propiedades derivadas y referencias de sistemas externos

## Proyecciones semánticas computadas

Un Space puede poseer propiedades derivadas de su valor aceptado, sin constituir otro concepto ni otro estado editable. Cuando el caso realmente requiere normalización, representar la proyección como `NormalizedValue` **computado, de solo lectura y dependiente exclusivamente del valor aceptado**; no crear automáticamente un `NormalizedActivityName` separado ni imponer `NormalizedValue` a todos los Spaces.

Distinguir **normalizar el valor aceptado durante `Transform`** de **derivar una representación normalizada** para comparar o indexar; no sobrescribir indiscriminadamente el valor original. La regla concreta (mayúsculas, Unicode, espacios, diacríticos, etc.) pertenece al significado del Space y debe obtenerse de la especificación. Si una funcionalidad exige normalización y no hay política suficiente, registrar la laguna sin inventar el algoritmo. Un `ToUpperInvariant()` mostrado en ejemplos es solo ilustrativo.

No confundir equivalencia bajo una proyección normalizada con igualdad del Space o identidad de la entidad. La Feature decide el tipo de búsqueda (prefijo, igualdad, etc.), y la infraestructura puede mantener una columna `NormalizedName` derivada para consulta: esa columna no adquiere autoridad semántica. Comprobar la concordancia entre normalización .NET y colaciones/operadores SQL si la búsqueda depende de ello.

**No definir todavía un trait genérico de normalización/búsqueda.** La responsabilidad está identificada, pero el contrato abstracto aún no se ha derivado de varios casos reales.

## Identidad propia frente a referencia externa

Cuando un Space representa una identidad **propia del sistema o ámbito semántico actual**, usar el nombre de identidad pertinente, como `ParticipantId`, conforme a las reglas del dominio.

Cuando un concepto mantiene una **referencia a un objeto cuya identidad y ciclo de vida pertenecen a otro sistema o autoridad**, seguir la convención VSlices de nombrar el concepto local `<Concept>Reference`: por ejemplo, `AccountReference` para una cuenta gestionada por un sistema externo, **no `AccountId` como si la identidad de Account fuera propia del modelo**.

El sufijo `Reference` comunica la relación y la autoridad externa; no exige por sí solo un formato, `Guid`, URL, protocolo, validación remota, copia del objeto externo ni un trait adicional. Distinguir identificadores naturales, identificadores artificiales propios y referencias externas. El formato y la evidencia de la referencia deben seguir el contrato de integración si existe; si no, registrar como no especificados y no aplicar automáticamente el `Guid` provisional reservado a identificadores artificiales propios.

La referencia externa se modela como valor semántico local; la resolución, existencia y disponibilidad del objeto remoto pertenecen a Features/infraestructura cuando requieren IO. No inventar consultas remotas en `Transform` o `Evolve`.

## Contrastes

- `ActivityName.NormalizedValue`: derivación del mismo Space; no nuevo `NormalizedActivityName` por defecto.
- `Activity.Id: ActivityId`: identidad de actividad propia.
- `Participant.Account: Option<AccountReference>`: vínculo opcional a un objeto de otro sistema cuando su ausencia sea válida; no `Option<AccountId>` por comodidad de infraestructura.
- `ParticipantId`: identidad local del participante si el modelo así lo define; no renombrarla automáticamente a `ParticipantReference`.

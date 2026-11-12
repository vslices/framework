# Oráculo privado — campaña de actividades, iteración 2

**NO compartir con el agente evaluado.** Evaluar por comportamiento y garantías demostrables, no por nombres exactos o una única implementación.

## Criterios

1. **Autoridad / consulta (0–2):** lee la Skill y referencias vigentes, verifica contratos de VSlices.Spaces; no consulta carpeta de pruebas ni presenta compilación no ejecutada.
2. **Conceptos nominales (0–2):** representa ActivityName, descripción, identificadores, capacidad, participante y colección con tipos semánticos; no deja `string Name` como concepto crudo dentro del estado compuesto.
3. **IDs artificiales (0–2):** utiliza `Guid` provisional por defecto para ActivityId/ParticipantId propios, reporta tipo y formato desconocidos; no aplica formato forzado a AccountReference.
4. **Referencia externa (0–2):** distingue AccountReference de AccountId local; la cuenta pertenece a otro sistema y no se valida remotamente desde Transform/Evolve; opcionalidad justificada.
5. **Opcionalidad y colección (0–2):** descripción ausente mediante `Option<Description>` o semántica equivalente y no vacía válida; cero inscritos válido; colección ordenada encapsulada con unicidad por ParticipantId.
6. **Capacidad (0–2):** capacidad positiva e inmutable durante la evolución; evita sobrecupo tanto al construir como al proponer inscripción.
7. **Normalización (0–2):** ActivityName mantiene Value y expone NormalizedValue derivado, no un nuevo Space separado por norma; se logra insensibilidad a mayúsculas, se reportan diacríticos y demás políticas sin decidir; distingue igualdad de coincidencia por prefijo.
8. **Evolvable estructural (0–2):** State/CurrentState según trait; ActivityId y Capacity son get-only, Name/Description/Participants solo init cuando pueden proponerse. No deduce que get-only comprueba procedencia.
9. **Evolución semántica (0–2):** separa Feature/Update/Evolve; Evolve valida identidad ante estados independientes, capacidad, orden append-only, unicidad; la instancia anterior permanece intacta. No inventa API nueva de Evolvable.
10. **Verificación (0–2):** pruebas válidas e inválidas, rutas adversarias vía Evolve/Update estado ajeno, ausencia y unicidad, normalización; Laws SM001–SM007 consultadas o ejecutadas; reporta límites de compilación.
11. **Informe final (0–2):** categorías distintas de incoherencia, vacío, decisión provisional, riesgo; evidencia y consecuencias; sin inventar restricciones.
12. **No sobreingeniería (0–2):** sin IO, persistencia, Features completas, traits especulativos ni integración remota.

**Máximo: 24 puntos.** Puntuación por criterio: 2 logrado y evidenciado; 1 parcial o solo declarado; 0 ausente/incorrecto. Un comportamiento compilado vale más que una declaración, pero registrar los dos ejes independientemente.

## Casos de contraste necesarios

- `Transform` acepta cero participantes y descripción ausente, rechaza descripción vacía/no significativa.
- `Transform` rechaza capacidad <= 0, duplicados y sobrecupo.
- `Update` acepta propuesta append-only válida y conserva orden.
- `Evolve` rechaza reemplazo/reordenación/borrado de participantes e identidad diferente aun si los dos estados fueron construidos válidamente por separado.
- `State` rechaza por compilación un `with { Id = ... }` y `with { Capacity = ... }`, no necesariamente el reemplazo completo vía función.
- La búsqueda conceptual por `ac` encuentra `ACT 1` y `ACt 2` sin reemplazar su grafía.
- No se impone formato de AccountReference, ni número máximo de cuentas, ni unicidad por cuenta externa.
- El reporte revela ausencia del contrato externo, las restricciones no especificadas y decisiones provisionales.

## Protocolo

Pasar exclusivamente `activity-campaign.md` como texto del caso y el enlace a la Skill (acceso permitido a la Skill y su código). No enviar oráculos privados, nombres de aciertos esperados ni evaluaciones anteriores. Registrar versión de agente, HEAD inspeccionado, código, comandos y resultados. Evaluar después de su entrega.

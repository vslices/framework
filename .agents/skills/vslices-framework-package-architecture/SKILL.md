---
name: vslices-framework-package-architecture
description: >-
  Revisar y evolucionar la organización de paquetes, proyectos, namespaces y
  dependencias de VSlices Framework. Usar al crear, mover, renombrar o dividir
  paquetes NuGet, assemblies, carpetas, contratos Work, álgebras o realizaciones
  Grounding; también al revisar vocabulario de distribución y organización del código.
---

# Arquitectura de paquetes de VSlices Framework

## Regla fundamental

Tratar como dimensiones independientes:
- **Package / proyecto distribuible:** unidad de distribución, referencias y dependencias.
- **Namespace:** organización conceptual y vocabulario de tipos públicos.
- **Carpeta:** organización física del código fuente.
- **Contrato / álgebra:** capacidad ejecutable declarada, no necesariamente su realización.
- **Realización:** implementación concreta de un contrato, con sus propias dependencias.

No derivar automáticamente un namespace del PackageId o de la carpeta. Varios assemblies pueden contribuir tipos a un mismo namespace. Comprobar referencias de proyectos y paquetes por separado de los imports del código C#.

## Acuerdos actuales para la reorganización del PR #197

Estos son acuerdos arquitectónicos de revisión; no declarar que la migración ya está implementada hasta verificar el repositorio.

| Package | Namespace público relevante | Responsabilidad |
| --- | --- | --- |
| `VSlices.Work` | `VSlices.Work.Algebra` | Álgebras generales como `AlgebraMix`, `ClockIO`, `DelayIO`; contratos generales de Work |
| `VSlices.Work.DataAccess` | `VSlices.Work.Algebra` | `PointReader`, `PointWriter`, `PointRemover`, `HasPoint*` |
| `VSlices.Grounding` | `VSlices.Grounding.Realizations` | `SystemTimeIO` y realizaciones nativas apropiadas |
| `VSlices.Grounding.DataAccess.EFCore` | `VSlices.Grounding.Realizations` | `EntityFrameworkPointIO`, adaptación de contratos Point* con EF Core |
| `VSlices.Extensions.EFCore` | Por verificar y decidir desde sus tipos | Extensiones funcionales de EF Core, independientes de la categoría Grounding |

Usar exactamente **DataAccess**, no `DataAcess`. **EFCore** es la abreviatura acordada en los nombres de paquete anteriores.

Dependencias previstas:
- `VSlices.Work.DataAccess` referencia `VSlices.Work`; `VSlices.Work` no debe requerir `VSlices.Work.DataAccess`.
- `VSlices.Grounding.DataAccess.EFCore` consume los contratos Point* del paquete especializado y puede utilizar `VSlices.Extensions.EFCore`.
- `VSlices.Extensions.EFCore` no debe depender de Work ni de Grounding sin presión observada; comprobar su grafo real.
- El hecho de que dos tipos compartan namespace no implica que estén disponibles sin referenciar sus assemblies.

`ClockIO` y `DelayIO` permanecen en Work: son capacidades temporales generales, realizables con el `TimeProvider` nativo de .NET. No crear `VSlices.Work.Temporal` solo por simetría. Integraciones con proveedores no nativos pueden justificar paquetes específicos. Un posible cambio de nombres de las interfaces temporales requiere decisión y verificación separadas: no considerarlo aprobado por el solo nombre del archivo `Temporal.cs`.

## Procedimiento de revisión

1. Consultar `AGENTS.md`, el código, los proyectos, los tests y el estado de la rama; distinguir propuesta de implementación.
2. Identificar responsabilidad semántica, dependencia introducida y superficie pública afectada; contrastar si la extracción elimina una dependencia no necesaria del núcleo.
3. Determinar por separado PackageId, nombre de proyecto/assembly, namespace y ubicación física; evitar subniveles redundantes como `VSlices.Work.DataAccess.Algebra` si el namespace conceptual aprobado es `VSlices.Work.Algebra`.
4. Revisar consumidores, `ProjectReference`, `PackageReference`, `using`, solución, CI, empaquetado NuGet, ejemplos, Skills y documentación.
5. Verificar compilación y pruebas antes de declarar realizada la migración. Registrar rupturas de compatibilidad por tipo: paquete, namespace, assembly o API.
6. Mantener `template-service` en su versión publicada hasta disponer del paquete nuevo; no fusionar PRs ni publicar paquetes sin autorización explícita.

No deducir transacciones, tracking, repositorios, durabilidad ni atomicidad de `Point*`. Mantener la distinción entre significado, contrato de ejecución y realización concreta.

## Referencias próximas

- `AGENTS.md` y `src/` del repositorio vigente.
- `.agents/skills/vslices-feature-development/SKILL.md`.
- `docs/point-algebras.md` y `docs/work/feature-modeling.md`.
- Para autoridad de la suite, consultar `vslices/vslices-suite`; esta Skill solo establece una convención operativa de Framework.

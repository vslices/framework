# Realizaciones consultadas y extensiones por evaluar

Las realizaciones de consumidores son **evidencia**, no contratos universales de VSlices. Revisar su versión de LanguageExt y contrastar con las APIs disponibles antes de copiar código.

## Hual: dominios distintos

- **Logística:** [`Tare.New`](https://github.com/HernanFAR/Hual/blob/master/LogisticsContext.Domain/Aggregates/Tare.cs) combina cuatro `Fin` mediante `Apply`. Sirve como caso de validaciones independientes.
- **Legales:** [`PdiBook.Module`](https://github.com/HernanFAR/Hual/blob/master/Legals.Domain/Aggregates/PdiBook/PdiBook.Module.cs) compone obtención de repositorio y lectura opcional con `OptionT<Eff<HualRuntime>,PdiBook>`.
- **Identidades:** [`Identity.cs`](https://github.com/HernanFAR/Hual/blob/master/Identities.Domain/Aggregates/Identity.cs) presenta `Option<Email>`, `Option<PhoneNumber>` y una jerarquía nominal `Identity` / `NaturalIdentity` / `LegalIdentity`. El archivo también contiene una operación sin implementar: no citarla como ejemplo ejecutable exitoso.
- **VSlices Framework:** [`Point Algebras`](https://github.com/vslices/framework/blob/master/docs/point-algebras.md) describe `OptionT<IO,POINT>` para lecturas opcionales.

## Extensiones específicas de Hual

Referencia: [`OptionT.Prelude.cs`](https://github.com/HernanFAR/Hual/blob/master/SharedContext.Domain/PreludeExtensions/OptionT.Prelude.cs).

- `IfNone`: ejecuta `Run()` sobre `K<OptionT<Eff<HualRuntime>>,A>`; si hay valor devuelve `Eff.Pure`, si no `Eff.Fail(error)`.
- `IfNoneExtract`: ejecuta el transformador y elige el valor existente o un `Eff` alternativo.
- `BiBindExtract`: recibe funciones para las ramas `Some` y `None`, y devuelve `Eff<HualRuntime,B>`.

Evaluar cada una por separado: comparar con APIs estándar de LanguageExt v5 y analizar el sentido de ausencia en los contratos consumidores. **No incorporar automáticamente** estas extensiones a la Skill como recomendación universal ni al Framework como API nueva. Si alguna resulta reutilizable, coordinar su implementación con la actualización de Framework (incremento 4), incluidas dependencias de proyecto como `VSlices.csproj` cuando proceda.

## Límites de evidencia

En la revisión del 13 de noviembre de 2026 se consultaron archivos concretos de Hual y documentación de Framework. La lectura intentada de `atom-dev-serviu/serviu/README.md` devolvió `404`; por tanto, no se atribuyen usos específicos a ese repositorio sin acceso verificable.

No extrapolar problemas de LanguageExt v4 a v5 ni publicar ejemplos como compilados si no se ejecutó la compilación.

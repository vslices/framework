# MaintainedSpace: selecciones reconocidas por el dominio

Consultar el contrato vigente en `src/VSlices.Spaces/MaintainedSpace.cs` antes de implementar. En el Framework actual:

```csharp
public interface MaintainedSpace<SELF> : DiscreteSpace<SELF>
    where SELF : MaintainedSpace<SELF>
{
    static abstract Seq<SELF> All { get; }
}
```

`All` representa el **conjunto completo de valores reconocidos** por el Space. La interfaz no incorpora persistencia, CRUD, actualización dinámica de un catálogo ni reglas adicionales de orden o aritmética. Heredar de `DiscreteSpace<SELF>` exige una igualdad semántica coherente.

## Cuándo corresponde

Usar `MaintainedSpace<SELF>` cuando el significado del dominio reconoce una selección de constantes explícitas y completas para ese concepto. Por ejemplo, si el levantamiento establece exclusivamente tres riesgos: `Bajo`, `Medio` y `Alto`, esa selección puede modelarse como un Space nominal cuyos valores admitidos son precisamente esos tres.

No crear un `MaintainedSpace` solo porque haya un `select` en una pantalla: las opciones pueden provenir de un catálogo administrable, permisos, otro servicio o datos contextuales. Tampoco inferir que todo `enum` técnico tiene significado semántico propio.

## Construcción, conjunto completo e igualdad

- Dar significado nominal a cada constante. Preservar las invariantes del concepto y las convenciones aplicables de `Transformable<SELF.Input,SELF>` para definiciones sujetas a las Laws.
- Exponer `All` como `Seq<SELF>` que contenga **todos y solo** los valores reconocidos; no incorporar opciones futuras hipotéticas, valores sentinela sin respaldo ni duplicados por equivalencia semántica.
- Asegurar que la construcción pública no acepte valores fuera del conjunto reconocido; no basta con declarar `All` si `Transform` permite otros casos.
- Mantener coherencia entre `Equals`, `GetHashCode`, `==`, `!=` y la pertenencia a `All`. No depender de igualdad por referencia accidental cuando valores equivalentes pueden reconstruirse.
- Si los nombres visibles o su normalización no están especificados, no inferir reglas de comparación por analogía; documentar las decisiones pendientes.
- No añadir `Evolvable` por el hecho de que otro Space contenedor pueda cambiar su selección. Un valor del conjunto, como `Risk.Alto`, no se convierte en un objeto mutable porque un Ticket pueda pasar de riesgo bajo a alto.

## Diferenciar otros mecanismos

| Caso | Representación pertinente |
| --- | --- |
| Conjunto cerrado y conocido de constantes del dominio | `MaintainedSpace<SELF>` |
| Selección exclusiva entre **variantes con datos propios** | Unión discriminada, `Either` o ADT nominal según el dominio; no automáticamente `MaintainedSpace` |
| Registros administrables que se crean, renombran o desactivan | Spaces con identidades e invariantes de catálogo; no `All` estático de constantes |
| Subconjunto semántico de otro Space o derivación | Examinar el contrato específico pertinente solo cuando exista presión real; no introducir `DerivedSpace` por rutina |

Una `Choice` de pseudocódigo no es por sí misma un contrato C#: validar las alternativas acordadas y el trait vigente antes de elegir la implementación.

## Pruebas mínimas

1. `All` contiene exactamente el conjunto reconocido y no duplica equivalentes.
2. Cada constante de `All` es admisible y respeta su igualdad semántica.
3. Valores no reconocidos se rechazan en cualquier vía pública de construcción.
4. Dos instancias de una misma opción son iguales y producen hashes compatibles.
5. El uso de la selección en otro Space no añade reglas de mutabilidad, creación de catálogos ni transición no declaradas.

Si todavía no está decidido si las opciones son completas, su procedencia, o si serán administrables, registrar el vacío y no presentar una lista parcial como `All` definitivo.

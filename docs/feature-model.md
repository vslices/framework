# Feature Model

Features are executable operations owned by Work. A Feature has an executable algebra and an explicit public input request. The caller constructs the request, while only the owning Feature constructs its response.

## Core contract

```csharp
public interface Feature<F, ALG, REQ, RES>
    where F : Feature<F, ALG, REQ, RES>
{
    static abstract Eff<ALG, RES> Get(REQ request);
}
```

Example:

```csharp
public sealed class ReadItem :
    Feature<ReadItem, ItemAlgebra, ReadItem.Request, ReadItem.Response>
{
    public sealed record Request(ItemId Id);

    public sealed record Response
    {
        private Response(Item item) => Item = item;
        public Item Item { get; }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern Response NewResponse(Item item);

    public static Eff<ItemAlgebra, Response> Get(Request request) =>
        Eff<ItemAlgebra, Response>.LiftIO(algebra =>
            algebra.Reader.Read(request.Id).Map(NewResponse));
}
```

The request is applied at `Get(request)`, producing an `Eff<ALG, Response>` that is executed later using the algebra supplied by Grounding. Request is not `Transformable`; it is the nominal input to a proposed Work execution. Response uses encapsulated private construction because it is the Feature-owned outcome, not an arbitrary DTO.

For structural checks and explicit limitations see [Feature Modeling Laws](work/feature-modeling.md).

## Runtime and composition

`ALG` is a module-owned algebra of executable capability atoms, e.g. `PointReader`, `PointWriter`, `PointRemover`. Do not infer durability, transactionality, or repository semantics from these capabilities.

A parent Feature can compose children directly:

```csharp
Eff<AlgebraMix<A, B>, Response>.LiftIO(mix =>
    Child.Get(new Child.Request(...))
        .RunIO(mix.A)
        .Map(child => NewResponse(child.Result)));
```

This keeps child Feature ownership and projects only the required algebra. It does not require `MapRuntime`, `MapRequest`, or `ContraMap`.

`Flow` remains an independent Framework abstraction, but no longer defines the Feature contract.

## Errors

Expected failures must remain explicit as `Error.New(...)` propagated by the effect or represented in modeled results. Throw .NET exceptions only for unexpected exceptional conditions. The broader `PointReader.Read` error policy is a separate review; this contract change does not claim to have revised it.

Presentation transforms/refines external inputs into public Feature Requests, runs effects against Grounding algebras, and maps the response to the transport. The transport does not own the Feature's semantic result.

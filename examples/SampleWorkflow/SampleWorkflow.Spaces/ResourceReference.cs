using ForkExt;
using VSlices.Spaces;
using VSlices.Spaces.Traits;

namespace SampleWorkflow.Spaces;

/// <summary>
/// Opaque reference to a resource owned outside the Todo semantic boundary.
/// The Todo surface does not know which external capability realizes the resource.
/// </summary>
public sealed record ResourceReference :
    DiscreteSpace<ResourceReference>,
    Transformable<string, ResourceReference>
{
    private ResourceReference(string value) =>
        Value = value;

    public string Value { get; }

    public static Fin<ResourceReference> Transform(string value) =>
        new ResourceReference(value);

    public override string ToString() =>
        Value;
}

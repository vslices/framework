namespace SampleFileRepo;

public sealed record SampleFileId(Guid Value)
{
    public override string ToString() =>
        Value.ToString();
}

public sealed record SampleFile(
    SampleFileId Id,
    string Name,
    byte[] Content);

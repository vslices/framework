namespace VSlices.Domain.Unions;


public abstract record Dimension;

public sealed record Weight : Dimension;

public sealed record Volumen : Dimension;

public sealed record Count : Dimension;

public sealed record Length : Dimension;

using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;
using VSlices.Domain.Traits;

namespace VSlices.Domain.Maintainers;


public sealed class Dimension : Maintainer<Dimension, string>
{
    private readonly string _value;

    public static Dimension Weight { get; } = new(nameof(Weight));

    public static Dimension Volumen { get; } = new(nameof(Volumen));

    public static Dimension Count { get; } = new(nameof(Count));

    public static Dimension Length { get; } = new(nameof(Length));

    private Dimension(string value) => _value = value;

    public static Seq<Dimension> All =>
    [
        Weight, 
        Volumen, 
        Count, 
        Length
    ];

    public bool Equals(Dimension? other) => 
        other?._value == _value;

    public string To() => _value;
}

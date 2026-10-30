using System;

namespace ForkExt.Attributes;

[AttributeUsage(AttributeTargets.Interface)]
public class TraitAttribute : Attribute
{
    public readonly string NameFormat;
    public TraitAttribute(string nameFormat) =>
        NameFormat = nameFormat;
}

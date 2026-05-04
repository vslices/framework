using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Domain.Rules;

public sealed class AllowedChars<S> : Rule<AllowedChars<S>, string>
    where S : Const<string>
{
    public static bool Check(string value) =>
        value.All(c => S.Value.Contains(c));
}

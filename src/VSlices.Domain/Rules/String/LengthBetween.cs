using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Domain.Rules;

public sealed class LengthBetween<MIN, MAX> 
    : Rule<LengthBetween<MIN, MAX>, string>
    where MIN : Const<int>
    where MAX : Const<int>
{
    public int Min => MIN.Value;

    public int Max => MAX.Value;

    public static bool Check(string value) => 
        value.Length >= MIN.Value && 
        value.Length <= MAX.Value;
}
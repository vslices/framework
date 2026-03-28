using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public QueryMatching<RT, A> MatchQuery() =>
        new(this);

    public ActionMatching<RT, A> MatchAction() =>
        new(this);
}

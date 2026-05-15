namespace VSlices.Application.Envs;

public interface HasMetric<RT> : Has<Eff<RT>, MetricIO>;

public partial record MetricEnv<RT>
    where RT : HasMetric<RT>
{
    static Eff<RT, MetricIO> metricIO =>
        Has<Eff<RT>, RT, MetricIO>.ask.As();


}

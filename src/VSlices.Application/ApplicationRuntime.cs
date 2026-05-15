using VSlices.Application.Envs;
using VSlices.Domain;

namespace VSlices.Application;

public interface ApplicationRuntime<RT> 
    : DomainRuntime<RT>,
      HasMetric<RT>,
      HasLog<RT>,
      HasTrace<RT> 
    where RT : ApplicationRuntime<RT>;

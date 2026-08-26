using VSlices.Domain.Products.Roles;
using VSlices.Products;

namespace Shared.Domain.Products;

public abstract class AppSpecificRole : vRole
{
    public abstract UserAccessRole UserAccessRole { get; }
}

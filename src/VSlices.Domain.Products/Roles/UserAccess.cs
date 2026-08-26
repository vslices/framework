using VSlices.Domain.Products.Roles;
using VSlices.Products;

namespace Shared.Domain.Products;

public abstract class UserAccessRole : vRole
{
    public abstract Product Product { get; }
}

public abstract class ProfileAccessRole : UserAccessRole
{
    public abstract Profile Profile { get; }
}

public abstract class ModuleProfileAccessRole : UserAccessRole
{
    public abstract Module Module { get; }

    public abstract Profile Profile { get; }
}

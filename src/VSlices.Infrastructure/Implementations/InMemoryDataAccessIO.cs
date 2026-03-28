using System;
using System.Collections.Generic;
using System.Text;
using VSlices.Domain.Environments.DataAccess;
using VSlices.Domain.Interfaces;

namespace VSlices.Infrastructure.Implementations;

public sealed class InMemoryDataAccessIO : DataAccessIO
{
    public IO<T> Get<T>() where T : IRepository => throw new NotImplementedException();
}

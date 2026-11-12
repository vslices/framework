# VSlices.Grounding.EntityFrameworkCore.Functional

Functional, effectful extensions for Entity Framework Core built on [LanguageExt](https://github.com/louthy/language-ext).

The package keeps ordinary query composition as ordinary EF Core composition and wraps operations that actually cross the I/O boundary in `IO` / `OptionT<IO, A>`.

## Install

```bash
dotnet add package VSlices.Grounding.EntityFrameworkCore.Functional
```

## Current surface

### Query execution

```text
AllIO
AnyIO
CountIO
LongCountIO
FirstIO
FirstOrNoneIO
SingleIO
SingleOrNoneIO
ToSeqIO
MinIO
MaxIO
SumIO
AverageIO
FindOrNoneIO
```

### Context and persistence

```text
CreateDbContextIO
AddIO
AddRangeIO
UpdateIO
RemoveIO
RemoveRangeIO
SaveChangesIO
ClearIO
DisposeIO
```

### Set-based operations

```text
ExecuteUpdateIO
ExecuteDeleteIO
```

### Transactions

```text
BeginTransactionIO
CommitIO
RollbackIO
DisposeIO
```

## Example

```csharp
using VSlices.Grounding.EntityFrameworkCore.Functional;

var users =
    await db.Users
        .Where(user => user.Active)
        .ToSeqIO()
        .RunAsync();

var updated =
    await db.Users
        .Where(user => user.Pending)
        .ExecuteUpdateIO(setters =>
            setters.SetProperty(user => user.Pending, false))
        .RunAsync();
```

Compositional EF Core operations such as `Where`, `Select`, `Include`, `AsNoTracking`, `OrderBy`, and `AsSplitQuery` remain ordinary EF Core APIs rather than being wrapped unnecessarily.

## Verification

Integration tests execute against a real PostgreSQL instance provisioned with Testcontainers. CI verifies query execution, aggregates, persistence operations, `Find` tracking behavior, set-based update/delete, and transaction commit/rollback semantics.

## Authors

- Hernán Alvarez
- VSlices Development

Published and maintained under VSlices.

## Status

The package is young and its API is still being validated through real consumers. Initial releases use the `0.x` version line.

## Integration provenance

Imported from [vslices/functional-ef-core](https://github.com/vslices/functional-ef-core) at commit `89d7d57b7c28de39960c633045e9465ba7d45eb7`, preserving its API and PostgreSQL integration tests. Package ID: `VSlices.Grounding.EntityFrameworkCore.Functional`.

# Feature Model

Features are the main executable unit of application behavior in VSlices.

A feature represents one vertical slice of behavior:

- one explicit input;
- one explicit output;
- explicit runtime capabilities;
- explicit effectful execution;
- explicit failure handling.

A feature is not a service, handler, controller, manager, helper, or use-case class.

A feature is a declaration of behavior that can be executed by different presentation adapters.

## Shape

A feature should be modeled as a function from a request to a `Flow`.

```txt
REQ -> Flow<RT, REQ, RES>
````

Conceptually, a `Flow` represents:

```txt
RT + REQ -> effectful result of RES
```

Where:

* `RT` is the runtime capability carrier;
* `REQ` is the input request;
* `RES` is the successful result;
* expected failures are represented explicitly;
* side effects are delayed and controlled.

## Request

The request type represents the external input needed by the feature.

Requests should be small, explicit, and feature-specific.

Prefer:

```csharp
public sealed record RegisterUser(
    string Email,
    string DisplayName
);
```

Avoid generic or shared request bags:

```csharp
public sealed record Request(
    Dictionary<string, object> Values
);
```

A request should not expose infrastructure concepts such as HTTP, database rows, message broker envelopes, or UI component state.

Presentation adapters are responsible for translating external input into the request type.

## Response

The response type represents the successful output of the feature.

Prefer explicit response types when the result has domain or application meaning:

```csharp
public sealed record RegisterUserResult(
    UserId UserId
);
```

For features that do not need to return data, use an explicit unit-like result instead of returning `null`.

## Runtime Capabilities

Features must declare the minimum runtime capabilities they need.

Prefer narrow constraints:

```csharp
where RT : HasClock, HasPersistence
```

Avoid broad runtime requirements unless the feature truly needs them:

```csharp
where RT : ApplicationRuntime
```

Capabilities should describe what the runtime can do, not which concrete service is used.

Good capability examples:

* `HasClock`
* `HasPersistence`
* `HasTransaction`
* `HasLog`
* `HasMetric`
* `HasTrace`
* `HasCurrentUser`

Bad feature dependencies:

* `UserService`
* `UserManager`
* `RepositoryHelper`
* `ApplicationServices`
* service locator access
* static/global infrastructure access

## Example

A feature should be readable as a small vertical slice.

```csharp
public sealed record RegisterUser(
    string Email,
    string DisplayName
);

public sealed record RegisterUserResult(
    UserId UserId
);

public static class RegisterUserFeature
{
    public static Flow<RT, RegisterUser, RegisterUserResult> Handle<RT>()
        where RT : HasClock, HasPersistence
        =>
        Flow.From<RT, RegisterUser, RegisterUserResult>(request =>
            from now in Clock.UtcNow<RT>()
            from email in EmailAddress.Create(request.Email).ToFlow<RT>()
            from displayName in DisplayName.Create(request.DisplayName).ToFlow<RT>()
            from user in User.Register(email, displayName, now).ToFlow<RT>()
            from _ in Users.Save<RT>(user)
            select new RegisterUserResult(user.Id)
        );
}
```

The exact helper names may vary, but the shape should remain stable:

```txt
request
  -> validate / construct domain values
  -> execute domain behavior
  -> use capabilities explicitly
  -> return explicit result
```

## Presentation Adapters

Presentation adapters must not contain business behavior.

They should only:

1. receive external input;
2. translate it into `REQ`;
3. execute the feature `Flow`;
4. translate the result into a presentation-specific response.

Example responsibilities:

| Adapter        | Responsibility                |
| -------------- | ----------------------------- |
| Web API        | HTTP request/response mapping |
| Worker         | queue/message mapping         |
| CLI            | command-line argument mapping |
| UI             | component event/state mapping |
| Event consumer | event envelope mapping        |

The feature itself should not know which adapter executes it.

## Error Handling

Expected failures must be modeled explicitly.

Do not throw exceptions for normal domain or application failures.

Prefer domain-specific errors:

```csharp
public sealed record EmailAlreadyRegistered(EmailAddress Email) : Expected;
public sealed record InvalidDisplayName(string Reason) : Expected;
```

Avoid stringly typed failures:

```csharp
"Invalid user"
"Something went wrong"
```

A feature should preserve error meaning across composition.

## Side Effects

Side effects must happen through runtime capabilities.

A feature may perform effects such as:

* reading the current time;
* saving data;
* publishing events;
* writing logs;
* recording metrics;
* calling external systems.

But those effects must remain explicit in the `Flow` and in the required `RT` constraints.

Do not perform uncontrolled side effects inside domain methods or static helpers.

## Transactions

Transactions are execution concerns.

If a feature requires transactional behavior, it should use an explicit transaction capability.

```csharp
public static Flow<RT, RegisterUser, RegisterUserResult> Handle<RT>()
    where RT : HasClock, HasPersistence, HasTransaction
    =>
    Transaction.In<RT, RegisterUser, RegisterUserResult>(
        Flow.From<RT, RegisterUser, RegisterUserResult>(request =>
            // feature body
        )
    );
```

Do not manually spread transaction mechanics throughout feature code.

Keep transaction boundaries visible.

## Testing

Features should be tested through their `Flow` API.

Prefer fake or test runtimes over mocking service classes.

```csharp
[Fact]
public async Task register_user_persists_a_valid_user()
{
    var rt = TestRuntime.Create()
        .WithFixedClock(FixedInstant)
        .WithInMemoryPersistence();

    var request = new RegisterUser(
        Email: "user@example.com",
        DisplayName: "User"
    );

    var result = await RegisterUserFeature
        .Handle<TestRuntime>()
        .Run(rt, request);

    result.ShouldBeSuccess();
}
```

A test should verify behavior through the same model used by production code:

```txt
RT + REQ -> Flow result
```

Do not introduce service wrappers only to make testing easier.

Improve the test runtime instead.

## Feature File Organization

A feature should be easy to understand in isolation.

Recommended structure:

```txt
Features/
  RegisterUser/
    RegisterUser.cs
    RegisterUserResult.cs
    RegisterUserFeature.cs
    RegisterUserErrors.cs
    RegisterUserTests.cs
```

For very small features, request, result, errors, and behavior may live in one file.

Prefer locality over premature layering.

## Rules

A feature must:

* be modeled around `Flow<RT, REQ, RES>`;
* receive explicit input through `REQ`;
* return explicit output through `RES`;
* declare minimum runtime capabilities through `RT`;
* model expected failures explicitly;
* keep side effects inside controlled effectful execution;
* remain independent from presentation adapters;
* be testable with a fake/test runtime.

A feature must not:

* depend directly on infrastructure concretions;
* resolve dependencies through service locators;
* use constructor-injected services;
* hide runtime requirements behind generic service objects;
* throw exceptions for expected failures;
* contain HTTP, UI, worker, or database-specific concerns;
* introduce shared orchestration layers without a real need.

## Deprecated Model

Do not introduce new usages of `FeatureEff<RT, A>`.

`FeatureEff<RT, A>` was an earlier execution abstraction and is deprecated.

New feature execution code must use:

```txt
Flow<RT, REQ, RES>
```

```

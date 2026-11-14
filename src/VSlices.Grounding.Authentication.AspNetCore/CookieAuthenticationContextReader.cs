using VSlices.Spaces.Constants;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VSlices.Spaces.Definitions;
using VSlices.Work.Algebra;

namespace VSlices.Grounding.Authentication.AspNetCore;

/// <summary>
/// Cookie authentication observation shared across ASP.NET Core hosts.
/// Uses ASP.NET Core's named CookieAuthenticationOptions rather than a custom
/// config; never decodes a cookie or issues credentials.
/// </summary>
public abstract class CookieAuthenticationContextReader(
    string scheme,
    IHttpContextAccessor contexts,
    IAuthenticationService authentication,
    IOptionsMonitor<CookieAuthenticationOptions> cookies,
    IServiceProvider services)
    : AuthenticationContextReader
{
    private static readonly AuthenticationMethod CookieMethod = AuthenticationMethod.Cookie;
    private static readonly AuthenticationMethod NoMethod = AuthenticationMethod.None;

    public IO<AuthenticationContext> Read() =>
        IO.liftAsync<AuthenticationContext>(async () =>
        {
            if (contexts.HttpContext is { } http)
            {
                var cookieName = cookies.Get(scheme).Cookie.Name;
                if (string.IsNullOrWhiteSpace(cookieName))
                    throw new InvalidOperationException(
                        "The configured cookie scheme requires a cookie name.");

                var result = await authentication.AuthenticateAsync(http, scheme);

                if (result.Succeeded)
                    return Authenticated(result.Principal);

                if (result.Failure is not null)
                    return Require(AuthenticationContext.Rejected.Transform(
                        new(CookieMethod)));

                // ASP.NET Core can return NoResult for invalid cookies. The
                // mere presence of a cookie cannot establish authentication.
                var method = http.Request.Cookies.ContainsKey(cookieName)
                    ? CookieMethod : NoMethod;
                return Require(AuthenticationContext.Unauthenticated.Transform(
                    new(method)));
            }

            // Interactive Server circuits are not ordinary HTTP requests.
            // Resolve the provider from the CURRENT circuit scope, never from
            // an application singleton or a cached HttpContext/principal.
            var provider = services.GetService<AuthenticationStateProvider>();
            if (provider is null)
                throw new InvalidOperationException(
                    "No HTTP context or scoped AuthenticationStateProvider is available.");

            var state = await provider.GetAuthenticationStateAsync();
            if (state.User?.Identity?.IsAuthenticated == true)
                return Authenticated(state.User);

            return Require(AuthenticationContext.Unauthenticated.Transform(
                new(NoMethod)));
        });

    private AuthenticationContext Authenticated(System.Security.Claims.ClaimsPrincipal? principal)
    {
        if (principal?.Identity?.IsAuthenticated != true)
            throw new InvalidOperationException(
                "Authentication succeeded without an authenticated principal.");

        var identity = Require(TransformIdentity(principal));
        return Require(AuthenticationContext.Authenticated.Transform(
            new(CookieMethod, identity)));
    }

    protected abstract Fin<VSlices.Spaces.Definitions.Identity> TransformIdentity(System.Security.Claims.ClaimsPrincipal principal);

    private static T Require<T>(Fin<T> value) =>
        value.Match(
            Succ: static result => result,
            Fail: error => throw new InvalidOperationException(
                "Authentication context transformation failed.", error.ToException()));
}

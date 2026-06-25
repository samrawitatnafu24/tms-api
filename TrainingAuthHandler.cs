using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

public class TrainingAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TrainingAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
    // 1. Bypass authentication for favicon, scalar documentation, and openapi json
var path = Request.Path.Value ?? "";
if (path.Contains("favicon.svg", StringComparison.OrdinalIgnoreCase) || 
    path.StartsWith("/scalar", StringComparison.OrdinalIgnoreCase) || 
    path.StartsWith("/openapi", StringComparison.OrdinalIgnoreCase))
{
    return Task.FromResult(AuthenticateResult.NoResult());
}
        
 // 2. Keep enforcing the security header for all other API requests
     if (!Request.Headers.ContainsKey("X-Training-User"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing training user header."));
        }

        // Extract header value to assemble identities and attach successful claims
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, Request.Headers["X-Training-User"]!)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
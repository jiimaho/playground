using System.Security.Claims;
using System.Text.Encodings.Web;
using Disasters.Api.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

public class AuthenticationHandler : AuthenticationHandler<AuthenticationHandlerOptions>
{
    public AuthenticationHandler(IOptionsMonitor<AuthenticationHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    public AuthenticationHandler(IOptionsMonitor<AuthenticationHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return Task.FromResult(
            AuthenticateResult.Success(
                new AuthenticationTicket(
                    new ClaimsPrincipal(
                        new List<ClaimsIdentity>()
                        {
                            new ClaimsIdentity(
                                new List<Claim>
                                {
                                    new Claim("name", "bob")
                                }, 
                                "MyScheme", 
                                "name", 
                                "role")
                        }), 
                    "MyScheme")));
    }
}
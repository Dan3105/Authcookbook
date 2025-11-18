From .NET 9 Source Code
![[Pasted image 20251111073732.png]]
The `SignIn` Flow in [[0100 Cookie Authentication|Cookie Authentication]] in .NET:
### Scenario 1: No `SessionStore` configured (Default)
When call `SignInAsync`, it creates an `AuthenticationTicket` containing the **`ClaimsPrincipal`** (user identity/claims) and `AuthenticationProperties` (expiration, persistence settings, etc.), encrypts the entire ticket using ASP.NET's Data Protection system, and **stores** it in the cookie sent to the client

```csharp
// What gets created internally:
var ticket = new AuthenticationTicket(
    principal: claimsPrincipal,           // Your user's claims
    properties: authProperties,            // IsPersistent, ExpiresUtc, etc.
    authenticationScheme: "Cookies"
);
// Entire ticket is encrypted and stored in **cookie**
```

### Scenario 2: With `SessionStore` configured
- The `HandleSignInAsync` **method builds up a new `AuthenticationTicket`**, and when `SessionStore` is configured, it stores the full ticket server-side and only stores a session identifier in the encrypted cookie sent to the client
```csharp
// What happens internally:
var fullTicket = new AuthenticationTicket(
    principal: claimsPrincipal,      // All your user's claims
    properties: authProperties,
    authenticationScheme: "Cookies"
);

// Store FULL ticket server-side, get back session ID
string sessionId = await Options.SessionStore.StoreAsync(fullTicket);

// Create MINIMAL ticket with only session ID
var minimalTicket = new AuthenticationTicket(
    principal: new ClaimsPrincipal(
        new ClaimsIdentity(
            new[] { new Claim("SessionId", sessionId) }
        )
    ),
    properties: null,
    authenticationScheme: "Cookies"
);

// Encrypt MINIMAL ticket (containing session ID) and store in cookie
```


Full source code in .NET 9
```csharp
protected override async Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
    {
        ArgumentNullException.ThrowIfNull(user);

        properties = properties ?? new AuthenticationProperties();

        _signInCalled = true;

        // Process the request cookie to initialize members like _sessionKey.
        await EnsureCookieTicket();
        var cookieOptions = BuildCookieOptions();

        var signInContext = new CookieSigningInContext(
            Context,
            Scheme,
            Options,
            user,
            properties,
            cookieOptions);

        DateTimeOffset issuedUtc;
        if (signInContext.Properties.IssuedUtc.HasValue)
        {
            issuedUtc = signInContext.Properties.IssuedUtc.Value;
        }
        else
        {
            issuedUtc = TimeProvider.GetUtcNow();
            signInContext.Properties.IssuedUtc = issuedUtc;
        }

        if (!signInContext.Properties.ExpiresUtc.HasValue)
        {
            signInContext.Properties.ExpiresUtc = issuedUtc.Add(Options.ExpireTimeSpan);
        }

        await Events.SigningIn(signInContext);

        if (signInContext.Properties.IsPersistent)
        {
            var expiresUtc = signInContext.Properties.ExpiresUtc ?? issuedUtc.Add(Options.ExpireTimeSpan);
            signInContext.CookieOptions.Expires = expiresUtc.ToUniversalTime();
        }

        var ticket = new AuthenticationTicket(signInContext.Principal!, signInContext.Properties, signInContext.Scheme.Name);

        if (Options.SessionStore != null)
        {
            if (_sessionKey != null)
            {
                // Renew the ticket in cases of multiple requests see: https://github.com/dotnet/aspnetcore/issues/22135
                await Options.SessionStore.RenewAsync(_sessionKey, ticket, Context, Context.RequestAborted);
            }
            else
            {
                _sessionKey = await Options.SessionStore.StoreAsync(ticket, Context, Context.RequestAborted);
            }

            var principal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[] { 
	                    new Claim(SessionIdClaim, _sessionKey, ClaimValueTypes.String, Options.ClaimsIssuer) 
					}, Options.ClaimsIssuer));
            ticket = new AuthenticationTicket(principal, null, Scheme.Name);
        }

        var cookieValue = Options.TicketDataFormat.Protect(ticket, GetTlsTokenBinding());

        Options.CookieManager.AppendResponseCookie(
            Context,
            Options.Cookie.Name!,
            cookieValue,
            signInContext.CookieOptions);

        var signedInContext = new CookieSignedInContext(
            Context,
            Scheme,
            signInContext.Principal!,
            signInContext.Properties,
            Options);

        await Events.SignedIn(signedInContext);

        // Only honor the ReturnUrl query string parameter on the login path
        var shouldHonorReturnUrlParameter = Options.LoginPath.HasValue && OriginalPath == Options.LoginPath;
        await ApplyHeaders(shouldRedirect: true, shouldHonorReturnUrlParameter, signedInContext.Properties);

        Logger.AuthenticationSchemeSignedIn(Scheme.Name);
    }
```
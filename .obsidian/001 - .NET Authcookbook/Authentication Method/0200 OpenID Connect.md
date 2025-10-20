Default workflow
![[Pasted image 20251026083003.png]]
PKCE: [Proof Key for Code Exchange by OAuth Public Clients](https://datatracker.ietf.org/doc/html/rfc7636)

> OpenID Connect comes in many variations and all server implementations have slightly different parameters and requirements.


### Code example
```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(options =>
{
    var oidcConfig = builder.Configuration.GetSection("OpenIDConnectSettings");

    options.Authority = oidcConfig["Authority"];
    options.ClientId = oidcConfig["ClientId"];
    options.ClientSecret = oidcConfig["ClientSecret"];

    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.ResponseType = OpenIdConnectResponseType.Code;

    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;

    options.MapInboundClaims = false;
    options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
    options.TokenValidationParameters.RoleClaimType = "roles";
});
```

### Something need to explain
#### OpenID and OAuth?
From Claude answer
##### OAuth 2.0:
- **Purpose**: Get **access** to user's resources on third-party services
- **Example**: Your app needs to read user's Google Drive files, post to their Facebook, access their payment info
- **What you get**: An **access_token** that lets you call APIs on their behalf
- **Use case**: "Let this app access my photos on Google Photos"
##### OpenID Connect (OIDC):

- **Purpose**: Get **identity** information (authenticate who the user is)
- **Example**: "Login with Google" - you just want to know WHO they are (email, name, user ID)
- **What you get**: An **id_token** (JWT with user identity) + optionally an access_token
- **Use case**: "I want to login using my Google account"

In code term
```
OAuth 2.0 flow gives you:
→ access_token (for API access)

OpenID Connect flow gives you:
→ access_token (still there)
→ id_token (NEW - contains who the user is)
```

#### Google/Github/Facebook etc...
Those method authentication from those system is using OpenID protocol, in .NET it will have a high-chance that 
~~`AddGoogle()`, `AddFacebook()`, `AddGithub()`... Are built based on/idea from `AddOpenId`~~
`AddGoogle()`, `AddFacebook()`, `AddGithub()`... Are built based on `AddOAuth`
### To Learn more
[OAuth 2 Simplified • Aaron Parecki](https://aaronparecki.com/oauth-2-simplified/)
[Configure OpenID Connect Web (UI) authentication in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-oidc-web-authentication?view=aspnetcore-9.0)
[asp.net core - .AddOAuth() vs .AddOpenIdConnect() - Stack Overflow](https://stackoverflow.com/questions/59958467/addoauth-vs-addopenidconnect)


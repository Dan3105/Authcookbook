## Definition
`JWT (JSON Web Token) Bearer Authentication is commonly utilized for APIs`

 A JWT is a self-contained token that encapsulates information for an API resource or a client. The client which requested the JWT can request data from an API resource using the Authorization header and a bearer token

JWT Bearer Authentication provides:
- **Authentication**: When using the `JwtBearerHandler`, bearer tokens are essential for authentication. The `JwtBearerHandler` validates the token and extracts the user's identity from its [[0001 Authentication#^6c7dc1|claims]].
- **Authorization**: Bearer tokens enable authorization by providing a collection of [[0001 Authentication#^6c7dc1|claims]] representing the user's or application's permissions, much like a cookie.
- **Delegated Authorization**: When a user-specific access token is used to authenticate between APIs instead of an application-wide access token, this process is known as _delegated authorization_.

### Vocabulary

| Term         | Explanation                                                                                    |
| ------------ | ---------------------------------------------------------------------------------------------- |
| **Issuer**   | **"who created/issued this token?"** or **"which authorization server generated this token?"** |
| **Audience** | **"who is this token intended for?"** or **"who should accept/validate this token?"**          |
|              |                                                                                                |


## Token Types
There are numerous types of tokens and formats. Generating your own access tokens or ID tokens is **discouraged**, except for testing purposes. Self-created tokens that do not adhere to established standards:
- Can lead to security vulnerabilities.
- Are only suitable for closed systems.

> recommend using [OpenID Connect 1.0](https://openid.net/specs/openid-connect-core-1_0-final.html) or an OAuth standard for creating access tokens intended for API access.

## Access tokens
Access tokens:
- Are strings used by a client app to make requests to the server implementing an API.
- Can vary in format. Different APIs may use different formats for the tokens.
- Can be encrypted.
- Should never be read or interpreted by a web client or UI app holding the access token.
- Are intended solely for making requests to an API.
- Are typically sent to the API in the **Authorization** request header as a bearer token

### Application access tokens and delegated access tokens
Access tokens can be either _application access tokens_ or _delegated access tokens_. The tokens have different claims and are managed and stored differently. 

An **_application access token_** is typically stored once in the app until it expires, while a **_delegated access token_** is stored per user, either in a cookie or in a secure server cache.

We recommend using delegated user access tokens whenever a user is involved. Downstream APIs can request a delegated user access token on behalf of the authenticated user.
### Sender constrained access tokens
Access tokens can be used as [bearer tokens](https://cloud.google.com/docs/authentication/token-types#bearer) or [sender-constrained tokens](https://docs.verify.ibm.com/ibm-security-verify-access/docs/tasks-certboundaccesstoken) to access resources. Sender-constrained tokens require the requesting client to prove possession of a private key to use the token. Proving possession of a private key guarantees the token can't be used independently. Sender-constrained tokens can be implemented in two ways:

- [Demonstrating Proof of Possession (DPoP)](https://datatracker.ietf.org/doc/html/rfc9449)
- [Mutual-TLS (MTLS)](https://datatracker.ietf.org/doc/html/rfc8705)

### ID tokens
ID tokens are security tokens that confirm a user’s successful authentication. The tokens allow the client to verify the user’s identity. The JWT token server issues ID tokens containing claims with user information. ID tokens are always in [JWT](https://jwt.io/introduction) format.

ID tokens _**should never**_ be used to access APIs.

#### ID Tokens vs [[#Access tokens|Access Token]]

|          | ID Token                                              | Access Token                                          |
| -------- | ----------------------------------------------------- | ----------------------------------------------------- |
| Purpose  | Proves WHO the user is (authentication)               | Grants permission to access resources (authorization) |
| Audience | Your client application (frontend)                    | The API/resource server                               |
| Data     | User profile information (name, email, picture, etc.) | Permissions/scopes, what the user can do              |
| Use case | Display user info in UI, client-side personalization  | Making API calls, accessing protected resources       |
| Format   | Always JWT                                            | Can be JWT or opaque reference token                  |
|          |                                                       |                                                       |

### What role has OIDC and/or OAuth when using bearer tokens?

When an API uses JWT access tokens for authorization, the API only validates the access token, not on how the token was obtained.

OpenID Connect (OIDC) and OAuth 2.0 provide standardized, secure frameworks for token acquisition. Token acquisition varies depending on the type of app. Due to the complexity of secure token acquisition, it's highly recommended to rely on these standards:

- **For apps acting on behalf of a user and an application**: [[0200 OpenID Connect|OIDC]] is the preferred choice, enabling delegated user access. In web apps, the confidential code flow with [Proof Key for Code Exchange](https://oauth.net/2/pkce/) (PKCE) is recommended for enhanced security.
    - If the calling app is an ASP.NET Core app with server-side [OIDC authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-oidc-web-authentication), you can use the [SaveTokens](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.remoteauthenticationoptions.savetokens) property to store access token in a cookie for later use via [`HttpContext.GetTokenAsync("access_token")`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationhttpcontextextensions.gettokenasync).
- If the app has no user: The OAuth 2.0 client credentials flow is suitable for obtaining application access tokens.

### Code example
#### Basic Validation
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(jwtOptions =>
{
	jwtOptions.Authority = "https://{--your-authority--}";
	jwtOptions.Audience = "https://{--your-audience--}";
});
```

#### Explicit validation
```csharp
builder.Services.AddAuthentication()
.AddJwtBearer("some-scheme", jwtOptions =>
{
	jwtOptions.MetadataAddress = builder.Configuration["Api:MetadataAddress"];
	// Optional if the MetadataAddress is specified
	jwtOptions.Authority = builder.Configuration["Api:Authority"];
	jwtOptions.Audience = builder.Configuration["Api:Audience"];
	jwtOptions.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateIssuerSigningKey = true,
		ValidAudiences = builder.Configuration.GetSection("Api:ValidAudiences").Get<string[]>(),
		ValidIssuers = builder.Configuration.GetSection("Api:ValidIssuers").Get<string[]>()
	};

	jwtOptions.MapInboundClaims = false;
});
```
### JWT with multiple schemes

APIs often need to accommodate access tokens from various issuers. Supporting multiple token issuers in an API can be accomplished by:

- **Separate APIs**: Create distinct APIs with dedicated authentication schemes for each issuer.
- [`AddPolicyScheme`](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/policyschemes?view=aspnetcore-9.0) This method can define multiple authentication schemes and implement logic to select the appropriate scheme based on token properties (e.g., issuer, claims). This approach allows for greater flexibility within a single API.

### Forcing the bearer authentication
[SetFallbackPolicy](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authorization.authorizationbuilder.setfallbackpolicy) can be used to require authentication for all requests **even** to endpoints without an `[Authorize]` attribute

Example
```csharp
var requireAuthPolicy = new AuthorizationPolicyBuilder()
	.RequireAuthenticatedUser()
	.Build();

builder.Services.AddAuthorizationBuilder()
	.SetFallbackPolicy(requireAuthPolicy);
```

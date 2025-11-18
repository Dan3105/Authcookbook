
| Vocabulary                                                                | Source                  |
| ------------------------------------------------------------------------- | ----------------------- |
| [[0001 Authentication#Authentication scheme\|Authentication Scheme]]      | [[0001 Authentication]] |
| [[0001 Authentication#An authentication handler\|Authentication Handler]] | [[0001 Authentication]] |
| [[0001 Authentication#Authenticate\|Authenticate]]                        | [[0001 Authentication]] |

Add Authentication **Middleware** services with using Cookie scheme. 
```c#
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) .AddCookie();
```
Note: default is using Cookie scheme - follow [[0001 Authentication#Authentication scheme|Authentication Scheme]]

#### Cookie Authentication configuration options
```c#
builder.Services
.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => { 
   options.ExpireTimeSpan = TimeSpan.FromMinutes(20); 
   options.SlidingExpiration = true; 
   options.AccessDeniedPath = "/Forbidden/"; 
});
```

[`CookieAuthenticationDefaults.AuthenticationScheme`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationdefaults.authenticationscheme#microsoft-aspnetcore-authentication-cookies-cookieauthenticationdefaults-authenticationscheme) provides a value of `"Cookies"` for the scheme. Any string value can be used that distinguishes the scheme.

The [`CookieAuthenticationOptions`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationoptions) class is used to configure the authentication provider options.

#### Cookie Policy Middleware
Use [`CookiePolicyOptions`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.cookiepolicyoptions) provided to the **Cookie Policy Middleware** to control global characteristics of cookie processing and hook into cookie processing handlers when cookies are appended or deleted.
==*Middleware is processed in the order it's added*==

default [MinimumSameSitePolicy](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.cookiepolicyoptions.minimumsamesitepolicy#microsoft-aspnetcore-builder-cookiepolicyoptions-minimumsamesitepolicy) value is [[0110 SameSiteMode|SameSiteMode]].Lax to permit OAuth2 authentication

### Create an authentication cookie 
```c#
await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
```

`SignInAsync` creates an encrypted cookie and adds it to the current response. If `AuthenticationScheme` isn't specified, the default scheme is used.
#### `AuthencticationProperties`
![[Pasted image 20251111074654.png]]
### Sign out
```csharp
// Clear the existing external cookie 
await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
```

### Tips and trick
#### If a user account is disabled in back-end systems:
- The app's cookie authentication system continues to process requests based on the authentication cookie.
- The user remains signed into the app as long as the authentication cookie is valid.
**Option 1**: To prevent that using [`ValidatePrincipal`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationevents.validateprincipal) event can be used to intercept and override validation of the cookie entity. 
==Downside: Validating the cookie on every request mitigates the risk of revoked users accessing the app.== *No recommend*

**Option 2**: One approach to cookie validation is based on keeping track of when the user database changes. If the database hasn't been changed since the user's cookie was issued, there's no need to re-authenticate the user if their cookie is still valid

#### Persistent cookies
You may want the cookie to persist across browser sessions. This persistence should only be enabled with explicit user consent with a "Remember Me" checkbox on sign in or a similar mechanism.

*If the cookie expires while the browser is closed, the browser clears the cookie once it's restarted.*

#### Absolute cookie expiration
An absolute expiration time can be set with [ExpiresUtc](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationproperties.expiresutc#microsoft-aspnetcore-authentication-authenticationproperties-expiresutc). To create a persistent cookie, `IsPersistent` must also be set. Otherwise, the cookie is created with a session-based lifetime and could expire either before or after the authentication ticket that it holds. When `ExpiresUtc` is set, it overrides the value of the [ExpireTimeSpan](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationoptions.expiretimespan#microsoft-aspnetcore-authentication-cookies-cookieauthenticationoptions-expiretimespan) option of [CookieAuthenticationOptions](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationoptions), if set.
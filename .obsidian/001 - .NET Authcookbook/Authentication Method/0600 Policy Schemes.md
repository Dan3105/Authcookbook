**Source**: [Policy schemes](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/policyschemes?view=aspnetcore-9.0).
### Definition
Authentication policy schemes make it easier to have a single logical authentication scheme potentially use multiple approaches
Authentication policy schemes make it:
- Easy to forward any authentication action to another scheme.
- Forward dynamically based on the request.

**Example when to use**:  a policy scheme might use Google authentication for challenges, and cookie authentication for everything else.

### Logic design
All authentication [[0001 Authentication#Authentication scheme|schemes]] that use derived [`AuthenticationSchemeOptions`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationschemeoptions)and the associated [`AuthenticationHandler<TOptions>`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.authenticationhandler-1):
- Are automatically policy schemes in ASP.NET Core 2.1 or later.
- Can be enabled via configuring the scheme's options.

**Example**:
[Policy schemes in ASP.NET Core | Example Implementation](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/policyschemes?view=aspnetcore-9.0)
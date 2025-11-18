From suggestions in documents
[IHttpContextAccessor/HttpContext in ASP.NET Core Blazor apps | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/httpcontext?view=aspnetcore-9.0)

> [IHttpContextAccessor](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.ihttpcontextaccessor) generally should be avoided with interactive rendering because a valid [HttpContext](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext) isn't always available.

> [IHttpContextAccessor](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.ihttpcontextaccessor) can be used during static server-side rendering (static SSR), for example in statically-rendered root components, and when [using a token handler for web API calls](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/additional-scenarios?view=aspnetcore-9.0#use-a-token-handler-for-web-api-calls) on the server. **We recommend avoiding [IHttpContextAccessor](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.ihttpcontextaccessor) when static SSR or code running on the server can't be guaranteed.**



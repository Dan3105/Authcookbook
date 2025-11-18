using AuthServer.Components;
using AuthServer.Extension;
using AuthServer.Infracstructure;

// Initialize third-party services for development environment\
await ThirdPartyConfiguration.InitThirdPartyServicesForDevelopmentEnv();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddLogging();

builder.Services.AddExternalServices(builder.Configuration);
builder.Services.AddCommonServices(builder.Environment);
builder.Services.AddAuthenticationServices(builder.Configuration);

var app = builder.Build();  

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // Seed data before the app starts handling requests.
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            var db = services.GetRequiredService<InMemoryDbContext>();
            db.SeedData();
            logger.LogInformation("Database seeding completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            // Depending on your policy you can rethrow to stop startup:
            // throw;
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

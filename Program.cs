using AuthCookbook.Components;
using AuthCookbook.Core.Plugins;
using AuthCookbook.Core.Plugins.HashPassword;
using AuthCookbook.Core.Services.Authentication.Registration;
using AuthCookbook.Core.Services.Users;
using AuthCookbook.Core.Shared.Repository;
using AuthCookbook.Core.Shared.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
ConfigureServices(builder.Services, builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<InMemoryDbContext>();
    dbContext.SeedData();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

void ConfigureServices(IServiceCollection services, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        services.AddDbContext<InMemoryDbContext>(o =>
        {
            o.UseInMemoryDatabase("AuthCookbookDb");
        });
    }
    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddTransient<IRepositoryManager, RepositoryManager>();
    services.AddTransient<IHashPasswordService, BasicHashPasswordService>();
    services.AddTransient<IRegistrationService, BasicRegistrationService>();
    services.AddTransient<IMonitoringService, MonitoringService>();
}
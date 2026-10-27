using Microsoft.EntityFrameworkCore;
using SupplyFlow.Application;
using SupplyFlow.Infrastructure;
using SupplyFlow.Infrastructure.Data;
using SupplyFlow.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.EnvironmentName);
builder.Services.AddScoped<SupplyFlow.Web.Services.DashboardService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SupplyFlowDbContext>();
    await context.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<SupplyFlow.Application.Services.ISupplyWorkflowService>();
    await seeder.SeedIfEmptyAsync();
}

app.UseStaticFiles();
app.UseAntiforgery();
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTimeOffset.UtcNow }));
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();

public partial class Program { }

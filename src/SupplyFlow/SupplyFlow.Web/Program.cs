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
}

app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();

public partial class Program { }

using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SupplyFlow.Api.Extensions;
using SupplyFlow.Application;
using SupplyFlow.Infrastructure;
using SupplyFlow.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.EnvironmentName);

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Testing")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services.ApplyMigrationsAsync();
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<SupplyFlow.Application.Services.ISupplyWorkflowService>();
    await seeder.SeedIfEmptyAsync();
}
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTimeOffset.UtcNow }));
app.Run();

public partial class Program { }

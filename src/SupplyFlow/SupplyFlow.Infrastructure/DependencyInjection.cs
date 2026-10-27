using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupplyFlow.Application.Contracts;
using SupplyFlow.Infrastructure.Data;
using SupplyFlow.Infrastructure.Repositories;

namespace SupplyFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        string environmentName)
    {
        var connectionString = configuration.GetConnectionString("Default");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'Default' is not configured.");

        services.AddDbContext<SupplyFlowDbContext>(options =>
        {
            if (environmentName == "Testing")
                options.UseInMemoryDatabase("SupplyFlowTests");
            else
                options.UseNpgsql(connectionString);
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ISupplyRepository, SupplyRepository>();
        services.AddScoped<ISupplyWorkflowRepository, SupplyWorkflowRepository>();
        return services;
    }
}

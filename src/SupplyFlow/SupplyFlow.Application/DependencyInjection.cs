using Microsoft.Extensions.DependencyInjection;
using SupplyFlow.Application.Services;

namespace SupplyFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<ISupplyService, SupplyService>();
        services.AddScoped<ISupplyWorkflowService, SupplyWorkflowService>();
        return services;
    }
}

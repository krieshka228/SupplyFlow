using Microsoft.AspNetCore.Mvc;
using SupplyFlow.Api.Extensions;
using SupplyFlow.Application.DTOs;
using SupplyFlow.Application.Services;

namespace SupplyFlow.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(IProductService service) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetProducts(CancellationToken cancellationToken) =>
        Results.Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<IResult> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        try { return Results.Ok(await service.GetByIdAsync(id, cancellationToken)); }
        catch (Exception exception) { return exception.ToProblem(); }
    }

    [HttpPost]
    public async Task<IResult> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var created = await service.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/products/{created.Id}", created);
        }
        catch (Exception exception) { return exception.ToProblem(); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IResult> UpdateProduct(Guid id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        try { return Results.Ok(await service.UpdateAsync(id, request, cancellationToken)); }
        catch (Exception exception) { return exception.ToProblem(); }
    }
}

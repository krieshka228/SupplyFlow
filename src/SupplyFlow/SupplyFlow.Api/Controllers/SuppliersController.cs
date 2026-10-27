using Microsoft.AspNetCore.Mvc;
using SupplyFlow.Api.Extensions;
using SupplyFlow.Application.DTOs;
using SupplyFlow.Application.Services;

namespace SupplyFlow.Api.Controllers;

[ApiController]
[Route("api/suppliers")]
public sealed class SuppliersController(ISupplierService service) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetSuppliers(CancellationToken cancellationToken) =>
        Results.Ok(await service.GetAllAsync(cancellationToken));

    [HttpPost]
    public async Task<IResult> CreateSupplier(CreateSupplierRequest request, CancellationToken cancellationToken)
    {
        try { return Results.Ok(await service.CreateAsync(request, cancellationToken)); }
        catch (Exception exception) { return exception.ToProblem(); }
    }
}

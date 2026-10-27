using Microsoft.AspNetCore.Mvc;
using SupplyFlow.Api.Extensions;
using SupplyFlow.Application.DTOs;
using SupplyFlow.Application.Services;

namespace SupplyFlow.Api.Controllers;

[ApiController]
[Route("api/supplies")]
public sealed class SuppliesController(ISupplyService service) : ControllerBase
{
    [HttpGet]
    public async Task<IResult> GetSupplies(CancellationToken cancellationToken) =>
        Results.Ok(await service.GetAllAsync(cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<IResult> GetSupply(Guid id, CancellationToken cancellationToken)
    {
        try { return Results.Ok(await service.GetByIdAsync(id, cancellationToken)); }
        catch (Exception exception) { return exception.ToProblem(); }
    }

    [HttpPost]
    public async Task<IResult> CreateSupply(CreateSupplyRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var created = await service.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/supplies/{created.Id}", created);
        }
        catch (Exception exception) { return exception.ToProblem(); }
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<IResult> Submit(Guid id, CancellationToken cancellationToken)
    {
        try { return Results.Ok(await service.SubmitAsync(id, cancellationToken)); }
        catch (Exception exception) { return exception.ToProblem(); }
    }

    [HttpPost("{id:guid}/transit")]
    public async Task<IResult> Transit(Guid id, CancellationToken cancellationToken)
    {
        try { return Results.Ok(await service.SendToTransitAsync(id, cancellationToken)); }
        catch (Exception exception) { return exception.ToProblem(); }
    }

    [HttpPost("{id:guid}/deliver")]
    public async Task<IResult> Deliver(Guid id, CancellationToken cancellationToken)
    {
        try { return Results.Ok(await service.MarkDeliveredAsync(id, cancellationToken)); }
        catch (Exception exception) { return exception.ToProblem(); }
    }

    [HttpPost("{id:guid}/accept")]
    public async Task<IResult> Accept(Guid id, CancellationToken cancellationToken)
    {
        try { return Results.Ok(await service.AcceptAsync(id, cancellationToken)); }
        catch (Exception exception) { return exception.ToProblem(); }
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<IResult> Reject(Guid id, [FromBody] SupplyActionRequest request, CancellationToken cancellationToken)
    {
        try { return Results.Ok(await service.RejectAsync(id, request.Reason ?? string.Empty, cancellationToken)); }
        catch (Exception exception) { return exception.ToProblem(); }
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        try { return Results.Ok(await service.CancelAsync(id, cancellationToken)); }
        catch (Exception exception) { return exception.ToProblem(); }
    }
}

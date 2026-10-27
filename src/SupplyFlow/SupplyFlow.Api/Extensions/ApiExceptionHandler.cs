using SupplyFlow.Application.Contracts;

namespace SupplyFlow.Api.Extensions;

public static class ApiExceptionHandler
{
    public static IResult ToProblem(this Exception exception)
    {
        return exception switch
        {
            EntityNotFoundException => Results.Problem(
                title: "Resource not found",
                detail: exception.Message,
                statusCode: StatusCodes.Status404NotFound),
            BusinessRuleException => Results.Problem(
                title: "Business rule violation",
                detail: exception.Message,
                statusCode: StatusCodes.Status400BadRequest),
            _ => Results.Problem(
                title: "Unexpected error",
                detail: "Please try again later.",
                statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}

using LoanTracker.Shared.Models;
using Microsoft.AspNetCore.Http;

namespace LoanTracker.Domain.Models;

public static class ResultExtensions
{
    public static IResult Execute<T>(this Result<T> result)
    {
        if (result.IsValidationError)
            return Results.BadRequest(result);

        if (result.IsNotFoundError)
            return Results.NotFound(result);

        if (result.IsError)
            return Results.StatusCode(500);

        return Results.Ok(result);
    }
}

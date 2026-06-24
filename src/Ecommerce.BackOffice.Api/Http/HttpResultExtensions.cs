using Ecommerce.BackOffice.SharedKernel.Results;

namespace Ecommerce.BackOffice.Api.Http;

public static class HttpResultExtensions
{
    public static IResult ToHttpResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return Results.NoContent();
        }

        if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
        {
            return Results.NotFound(new { error = result.Error });
        }

        return Results.BadRequest(new { error = result.Error });
    }

    public static IResult ToHttpResult<T>(this Result<T> result, Func<T, string>? locationFactory = null)
    {
        if (result.IsFailure || result.Value is null)
        {
            if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Results.NotFound(new { error = result.Error });
            }

            return Results.BadRequest(new { error = result.Error });
        }

        return locationFactory is null
            ? Results.Ok(result.Value)
            : Results.Created(locationFactory(result.Value), result.Value);
    }
}

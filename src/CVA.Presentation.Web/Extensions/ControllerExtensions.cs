namespace CVA.Presentation.Web;

/// <summary>
/// Provides HTTP mapping extensions for <see cref="Result{T}"/>.
/// </summary>
public static class ControllerExtensions
{
    /// <summary>
    /// Maps a <see cref="Result{T}"/> to a standard HTTP response.
    /// Success -> 200 OK.
    /// Failure -> ProblemDetails (status from error.HttpStatus).
    /// </summary>
    public static ActionResult ToActionResult<T>(this ControllerBase controller, Result<T> result)
        => result.IsSuccess
            ? controller.Ok(result.Value)
            : MapError(controller, result);

    /// <summary>
    /// Maps a <see cref="Result{T}"/> to a 201 Created response when successful.
    /// Failure -> ProblemDetails.
    /// </summary>
    public static ActionResult ToCreatedAtActionResult<T>(this ControllerBase controller, Result<T> result, string actionName, object? routeValues)
        => result.IsSuccess
            ? controller.CreatedAtAction(actionName, routeValues, result.Value)
            : MapError(controller, result);

    private static ActionResult MapError<T>(ControllerBase controller, Result<T> result)
    {
        var error = result.Error!;
        var statusCode = error.Code switch
        {
            "NotFound" => StatusCodes.Status404NotFound,
            "Validation" => StatusCodes.Status400BadRequest,
            "Conflict" => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };

        return controller.Problem(
            detail: error.Message,
            statusCode: statusCode,
            title: error.Code);
    }
}
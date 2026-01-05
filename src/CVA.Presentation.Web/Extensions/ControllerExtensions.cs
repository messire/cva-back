namespace CVA.Presentation.Web;

/// <summary>
/// Provides HTTP mapping extensions for <see cref="Result{T}"/>.
/// </summary>
public static class ControllerExtensions
{
    extension(ControllerBase controller)
    {
        /// <summary>
        /// Maps a <see cref="Result{T}"/> to a standard HTTP response.
        /// Success -> 200 OK.
        /// Failure -> ProblemDetails (status from error.HttpStatus).
        /// </summary>
        public ActionResult<T> ToActionResult<T>(Result<T> result)
            => result.IsSuccess
                ? controller.Ok(result.Value)
                : controller.StatusCode(StatusCodes.Status400BadRequest, result.Error);

        /// <summary>
        /// Maps a <see cref="Result{T}"/> to a 201 Created response when successful.
        /// Failure -> ProblemDetails.
        /// </summary>
        public ActionResult<T> ToCreatedAtActionResult<T>(Result<T> result, string actionName, object? routeValues)
            => result.IsSuccess
                ? controller.CreatedAtAction(actionName, routeValues, result.Value)
                : controller.StatusCode(StatusCodes.Status400BadRequest, result.Error);
    }
}
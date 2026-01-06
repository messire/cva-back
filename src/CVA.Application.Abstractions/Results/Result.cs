using CVA.Application.Abstractions.Errors;

namespace CVA.Application.Abstractions;

/// <summary>
/// The result of an operation, encapsulating success status, value, and error information.
/// </summary>
/// <typeparam name="T">The type of the value returned in a successful result.</typeparam>
public sealed class Result<T>
{
    /// <summary>
    /// The value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// The value associated with a successful operation result.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// The error associated with a failed operation.
    /// </summary>
    public AppError? Error { get; }

    private Result(bool success, T? value, AppError? error)
    {
        IsSuccess = success;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Creates a successful <see cref="Result{T}"/> instance containing the specified value.
    /// </summary>
    public static Result<T> Ok(T value) => new(true, value, null);

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> instance containing the specified error.
    /// </summary>
    public static Result<T> Fail(AppError error) => new(false, default, error);

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> instance containing the specified error message with a generic failure code.
    /// </summary>
    public static Result<T> Fail(string message) => Fail(AppError.Failure(message));

    /// <summary>
    /// Implicitly converts a value to a successful result.
    /// </summary>
    public static implicit operator Result<T>(T value) => Ok(value);

    /// <summary>
    /// Implicitly converts an error to a failed result.
    /// </summary>
    public static implicit operator Result<T>(AppError error) => Fail(error);
}
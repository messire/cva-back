namespace CVA.Application.Contracts;

/// <summary>
/// The result of an operation, encapsulating success status, value, and error information.
/// </summary>
/// <typeparam name="T">The type of the value returned in a successful result.</typeparam>
public sealed class Result<T>
{
    /// <summary>
    /// The value indicating whether the operation was successful.
    /// </summary>
    /// <value>
    /// <c>true</c> if the operation succeeded; otherwise, <c>false</c>.
    /// </value>
    public bool IsSuccess { get; }

    /// <summary>
    /// The value associated with a successful operation result.
    /// </summary>
    /// <value>
    /// The value returned when the operation is successful; <c>null</c> if the operation failed.
    /// </value>
    public T? Value { get; }

    /// <summary>
    /// The error message associated with a failed operation.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> representing the error details when the operation is unsuccessful; otherwise, <c>null</c>.
    /// </value>
    public string? Error { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    private Result(bool success, T? value, string? error)
    {
        IsSuccess = success;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Creates a successful <see cref="Result{T}"/> instance containing the specified value.
    /// </summary>
    /// <param name="value">The value to encapsulate in the successful result.</param>
    /// <returns>A successful result containing the given value.</returns>
    public static Result<T> Ok(T value)
        => new(true, value, null);

    /// <summary>
    /// Creates a failed <see cref="Result{T}"/> instance containing the specified error message.
    /// </summary>
    /// <param name="error">The error message describing the failure.</param>
    /// <returns>A failed result containing the specified error message.</returns>
    public static Result<T> Fail(string error)
        => new(false, default, error);
}
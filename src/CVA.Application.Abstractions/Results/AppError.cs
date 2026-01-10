namespace CVA.Application.Abstractions;

/// <summary>
/// Represents an application error with a code and a descriptive message.
/// </summary>
public record AppError(string Code, string Message)
{
    /// <summary>
    /// Represents no error.
    /// </summary>
    public static readonly AppError None = new(string.Empty, string.Empty);
    
    /// <summary>
    /// Creates a validation error.
    /// </summary>
    public static AppError Validation(string message) => new("Validation", message);
    
    /// <summary>
    /// Creates a "Not Found" error.
    /// </summary>
    public static AppError NotFound(string message) => new("NotFound", message);
    
    /// <summary>
    /// Creates a generic failure error.
    /// </summary>
    public static AppError Failure(string message) => new("Failure", message);
    
    /// <summary>
    /// Creates a conflict error.
    /// </summary>
    public static AppError Conflict(string message) => new("Conflict", message);
}

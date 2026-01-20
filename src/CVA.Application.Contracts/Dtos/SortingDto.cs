namespace CVA.Application.Contracts;

/// <summary>
/// Applied sorting information for catalog responses.
/// </summary>
public sealed class SortingDto
{
    /// <summary>
    /// Sorting field key applied to the result set
    /// </summary>
    public required string Field { get; init; }

    /// <summary>
    /// Sorting order applied to the result set
    /// </summary>
    public required string Order { get; init; }
}
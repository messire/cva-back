namespace CVA.Application.Contracts;

/// <summary>
/// Represents a request to update the profile summary in a developer's profile.
/// </summary>
public sealed record UpdateProfileSummaryRequest
{
    /// <summary>
    /// The updated summary of the developer's profile.
    /// </summary>
    public string? Summary { get; init; }
}
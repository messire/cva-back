namespace CVA.Domain.Models;

/// <summary>
/// Represents a date range.
/// </summary>
/// <param name="Start">The start date of the range. </param>
/// <param name="End">The end date of the range, or null if the range is ongoing. </param>
public sealed record DateRange(DateOnly Start, DateOnly? End)
{
    /// <summary>
    /// Creates a new <see cref="DateRange"/> instance from the specified start and end dates.
    /// </summary>
    /// <param name="start">The start date of the range.</param>
    /// <param name="end">The end date of the range, or null if the range is ongoing.</param>
    /// <returns>The created date range.</returns>
    /// <exception cref="ArgumentException">Thrown if the end date is before the start date.</exception>
    public static DateRange From(DateOnly start, DateOnly? end)
        => end is null || end.Value >= start
            ? new DateRange(start, end)
            : throw new ArgumentException("End before start.", nameof(end));
}
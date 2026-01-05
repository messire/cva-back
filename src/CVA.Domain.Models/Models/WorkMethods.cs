namespace CVA.Domain.Models;

public sealed partial class Work
{
    /// <summary>
    /// Creates a new work experience entry.
    /// Purpose: enforce basic invariants at creation time.
    /// </summary>
    public static Work Create(
        string? companyName = null,
        string? role = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        string? description = null,
        string? location = null,
        IEnumerable<string>? achievements = null,
        IEnumerable<string>? techStack = null)
    {
        var work = new Work();
        work.Update(companyName, role, startDate, endDate, description, location, achievements, techStack);
        return work;
    }

    /// <summary>
    /// Updates work experience details.
    /// Purpose: allow controlled mutation while keeping invariants valid.
    /// </summary>
    public void Update(
        string? companyName = null,
        string? role = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        string? description = null,
        string? location = null,
        IEnumerable<string>? achievements = null,
        IEnumerable<string>? techStack = null)
    {
        EnsureValidDates(startDate, endDate);

        CompanyName = companyName?.Trim();
        Role = role?.Trim();
        Description = description?.Trim();
        Location = location?.Trim();

        StartDate = startDate;
        EndDate = endDate;

        ReplaceList(_achievements, achievements);
        ReplaceList(_techStack, techStack);
    }

    private static void EnsureValidDates(DateOnly? startDate, DateOnly? endDate)
    {
        if (!startDate.HasValue || !endDate.HasValue || endDate.Value >= startDate.Value) return;
        throw new DomainValidationException("EndDate must be greater or equal to StartDate.");
    }

    private static void ReplaceList(List<string> target, IEnumerable<string>? source)
    {
        target.Clear();
        if (source is null) return;

        foreach (var item in source.Where(i => !string.IsNullOrWhiteSpace(i)))
        {
            target.Add(item.Trim());
        }
    }
}
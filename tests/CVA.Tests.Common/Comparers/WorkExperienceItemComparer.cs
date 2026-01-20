using CVA.Tools.Common;

namespace CVA.Tests.Common.Comparers;

/// <summary>
/// Provides functionality to compare two <see cref="WorkExperienceItem"/> objects for equality.
/// </summary>
/// <seealso cref="IEqualityComparer{T}"/>
public class WorkExperienceItemComparer : IEqualityComparer<WorkExperienceItem>
{
    /// <summary>
    /// Determines whether two <see cref="WorkExperienceItem"/> objects are equal based on their properties.
    /// </summary>
    /// <param name="x">The first <see cref="WorkExperienceItem"/> object to compare.</param>
    /// <param name="y">The second <see cref="WorkExperienceItem"/> object to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the specified <see cref="WorkExperienceItem"/> objects are equal; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(WorkExperienceItem? x, WorkExperienceItem? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return x.Id == y.Id &&
               x.Company == y.Company &&
               x.Location == y.Location &&
               x.Role == y.Role &&
               x.Description == y.Description &&
               x.Period == y.Period &&
               x.TechStack.ScrambledEquals(y.TechStack);
    }

    /// <inheritdoc />
    public int GetHashCode(WorkExperienceItem? obj)
    {
        if (obj is null) return 0;
        var hash = new HashCode();
        hash.Add(obj.Id);
        hash.Add(obj.Company);
        hash.Add(obj.Location);
        hash.Add(obj.Role);
        hash.Add(obj.Description);
        hash.Add(obj.Period);
        obj.TechStack.ForEach(hash.Add);
        return hash.ToHashCode();
    }
}
using CVA.Domain.Models;
using CVA.Tools.Common;

namespace CVA.Tests.Common.Comparers;

/// <summary>
/// Provides functionality to compare two <see cref="Work"/> objects for equality.
/// </summary>
/// <seealso cref="IEqualityComparer{T}"/>
public class WorkComparer : IEqualityComparer<Work>
{
    /// <summary>
    /// Determines whether two <see cref="Work"/> objects are equal based on their properties.
    /// </summary>
    /// <param name="x">The first <see cref="Work"/> object to compare.</param>
    /// <param name="y">The second <see cref="Work"/> object to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the specified <see cref="Work"/> objects are equal; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(Work? x, Work? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return string.Equals(x.CompanyName, y.CompanyName) &&
               string.Equals(x.Role, y.Role) &&
               string.Equals(x.Description, y.Description) &&
               string.Equals(x.Location, y.Location) &&
               x.StartDate == y.StartDate &&
               x.EndDate == y.EndDate &&
               x.Achievements.ScrambledEquals(y.Achievements) &&
               x.TechStack.ScrambledEquals(y.TechStack);
    }

    /// <inheritdoc />
    public int GetHashCode(Work? obj)
    {
        if (obj is null) return 0;

        var hash = new HashCode();
        hash.Add(obj.CompanyName);
        hash.Add(obj.Role);
        hash.Add(obj.Description);
        hash.Add(obj.Location);
        hash.Add(obj.StartDate);
        hash.Add(obj.EndDate);

        obj.Achievements.ForEach(hash.Add);
        obj.TechStack.ForEach(hash.Add);

        return hash.ToHashCode();
    }
}
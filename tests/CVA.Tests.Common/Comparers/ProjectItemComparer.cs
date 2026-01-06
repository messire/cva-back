using CVA.Domain.Models;
using CVA.Tools.Common;

namespace CVA.Tests.Common.Comparers;

/// <summary>
/// Provides functionality to compare two <see cref="ProjectItem"/> objects for equality.
/// </summary>
/// <seealso cref="IEqualityComparer{T}"/>
public class ProjectItemComparer : IEqualityComparer<ProjectItem>
{
    /// <summary>
    /// Determines whether two <see cref="ProjectItem"/> objects are equal based on their properties.
    /// </summary>
    /// <param name="x">The first <see cref="ProjectItem"/> object to compare.</param>
    /// <param name="y">The second <see cref="ProjectItem"/> object to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the specified <see cref="ProjectItem"/> objects are equal; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(ProjectItem? x, ProjectItem? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return x.Id == y.Id &&
               x.Name == y.Name &&
               x.Description == y.Description &&
               x.Icon == y.Icon &&
               x.Link == y.Link &&
               x.TechStack.ScrambledEquals(y.TechStack);
    }

    /// <inheritdoc />
    public int GetHashCode(ProjectItem? obj)
    {
        if (obj is null) return 0;
        var hash = new HashCode();
        hash.Add(obj.Id);
        hash.Add(obj.Name);
        hash.Add(obj.Description);
        hash.Add(obj.Icon);
        hash.Add(obj.Link);
        obj.TechStack.ForEach(hash.Add);
        return hash.ToHashCode();
    }
}
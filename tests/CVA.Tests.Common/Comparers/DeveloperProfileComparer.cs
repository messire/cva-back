using CVA.Tools.Common;

namespace CVA.Tests.Common.Comparers;

/// <summary>
/// Provides functionality to compare two <see cref="DeveloperProfile"/> objects for equality.
/// </summary>
/// <seealso cref="IEqualityComparer{T}"/>
public class DeveloperProfileComparer : IEqualityComparer<DeveloperProfile>
{
    private static readonly ProjectItemComparer ProjectComp = new();
    private static readonly WorkExperienceItemComparer WorkExpComp = new();

    /// <summary>
    /// Determines whether two <see cref="DeveloperProfile"/> objects are equal based on their properties.
    /// </summary>
    /// <param name="x">The first <see cref="DeveloperProfile"/> object to compare.</param>
    /// <param name="y">The second <see cref="DeveloperProfile"/> object to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the specified <see cref="DeveloperProfile"/> objects are equal; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(DeveloperProfile? x, DeveloperProfile? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return x.Id == y.Id &&
               x.Name == y.Name &&
               x.Role == y.Role &&
               x.Summary == y.Summary &&
               x.Avatar == y.Avatar &&
               x.OpenToWork == y.OpenToWork &&
               x.Contact == y.Contact &&
               x.Social == y.Social &&
               x.Verification == y.Verification &&
               Math.Abs((x.CreatedAt - y.CreatedAt).TotalSeconds) < 1 &&
               Math.Abs((x.UpdatedAt - y.UpdatedAt).TotalSeconds) < 1 &&
               x.Skills.ScrambledEquals(y.Skills) &&
               x.Projects.ScrambledEquals(y.Projects, ProjectComp) &&
               x.WorkExperience.ScrambledEquals(y.WorkExperience, WorkExpComp);
    }

    /// <inheritdoc />
    public int GetHashCode(DeveloperProfile? obj)
    {
        if (obj is null) return 0;
        var hash = new HashCode();
        hash.Add(obj.Id);
        hash.Add(obj.Name);
        hash.Add(obj.Role);
        hash.Add(obj.Summary);
        hash.Add(obj.Avatar);
        hash.Add(obj.OpenToWork);
        hash.Add(obj.Contact);
        hash.Add(obj.Social);
        hash.Add(obj.Verification);

        obj.Skills.ForEach(hash.Add);
        obj.Projects.ForEach(item => hash.Add(item, ProjectComp));
        obj.WorkExperience.ForEach(item => hash.Add(item, WorkExpComp));

        return hash.ToHashCode();
    }
}
using CVA.Domain.Models;
using CVA.Tools.Common;

namespace CVA.Tests.Common.Comparers;

/// <summary>
/// Provides functionality to compare two <see cref="User"/> objects for equality.
/// </summary>
/// <seealso cref="IEqualityComparer{T}"/>
public class UserComparer : IEqualityComparer<User>
{
    private static readonly WorkComparer WorkComp = new();

    /// <summary>
    /// Determines whether two <see cref="User"/> objects are equal based on their properties.
    /// </summary>
    /// <param name="x">The first <see cref="User"/> object to compare.</param>
    /// <param name="y">The second <see cref="User"/> object to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the specified <see cref="User"/> objects are equal; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(User? x, User? y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (x is null || y is null) return false;

        return x.Id == y.Id &&
               string.Equals(x.Name, y.Name) &&
               string.Equals(x.Surname, y.Surname) &&
               string.Equals(x.Email, y.Email) &&
               string.Equals(x.Phone, y.Phone) &&
               string.Equals(x.Photo, y.Photo) &&
               string.Equals(x.SummaryInfo, y.SummaryInfo) &&
               x.Birthday == y.Birthday &&
               x.Skills.ScrambledEquals(y.Skills) &&
               x.WorkExperience.ScrambledEquals(y.WorkExperience, WorkComp);
    }

    /// <inheritdoc />
    public int GetHashCode(User? obj)
    {
        if (obj is null) return 0;

        var hash = new HashCode();
        hash.Add(obj.Id);
        hash.Add(obj.Name);
        hash.Add(obj.Surname);
        hash.Add(obj.Email);
        hash.Add(obj.Phone);
        hash.Add(obj.Photo);
        hash.Add(obj.SummaryInfo);
        hash.Add(obj.Birthday);

        obj.Skills.ForEach(hash.Add);
        obj.WorkExperience.ForEach(item => hash.Add(item, WorkComp));

        return hash.ToHashCode();
    }
}
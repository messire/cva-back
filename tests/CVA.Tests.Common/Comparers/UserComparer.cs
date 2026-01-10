namespace CVA.Tests.Common.Comparers;

/// <summary>
/// Provides functionality to compare two <see cref="User"/> objects for equality.
/// </summary>
/// <seealso cref="IEqualityComparer{T}"/>
public class UserComparer : IEqualityComparer<User>
{
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
               string.Equals(x.GoogleSubject, y.GoogleSubject) &&
               Equals(x.Role, y.Role) &&
               Equals(x.Email, y.Email);
    }

    /// <inheritdoc />
    public int GetHashCode(User? obj)
    {
        if (obj is null) return 0;

        var hash = new HashCode();
        hash.Add(obj.Id);
        hash.Add(obj.GoogleSubject);
        hash.Add(obj.Role);
        hash.Add(obj.Email);
        return hash.ToHashCode();
    }
}
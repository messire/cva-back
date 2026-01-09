namespace CVA.Tests.Common;

/// <summary>
/// A builder class that creates instances of the <see cref="User"/> type using the AutoFixture library.
/// </summary>
internal sealed class UserBuilder : ISpecimenBuilder
{
    /// <summary>
    /// A singleton instance of the <see cref="UserBuilder"/> class.
    /// </summary>
    public static readonly ISpecimenBuilder Instance = new UserBuilder();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(User)) return new NoSpecimen();

        var googleSubject = context.Resolve(new SeededRequest(typeof(string), nameof(User.GoogleSubject))).ToString()?.Split('-')[0];
        var email = (EmailAddress)context.Resolve(typeof(EmailAddress));
        var user = User.CreateFromGoogle(email.Value, googleSubject!, UserRole.User);
        return user;
    }
}
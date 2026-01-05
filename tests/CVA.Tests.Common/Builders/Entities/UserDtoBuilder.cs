using CVA.Application.Contracts;

namespace CVA.Tests.Common;

/// <summary>
/// A builder class that creates instances of the <see cref="UserDto"/> type using the AutoFixture library.
/// </summary>
internal sealed class UserDtoBuilder : ISpecimenBuilder
{
    /// <summary>
    /// A singleton instance of the <see cref="UserDtoBuilder"/> class.
    /// </summary>
    public static readonly ISpecimenBuilder Instance = new UserDtoBuilder();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(UserDto)) return new NoSpecimen();

        var name = (string)context.Resolve(typeof(string));
        var surname = (string)context.Resolve(typeof(string));
        var email = $"{name}_{surname}@example.test".ToLower();

        return new UserDto(name, surname, email)
        {
            Id = (Guid)context.Resolve(typeof(Guid)),
            Birthday = (DateOnly)context.Resolve(typeof(DateOnly)),
            Phone = (string)context.Resolve(typeof(string)),
            Photo = (string)context.Resolve(typeof(string)),
            SummaryInfo = (string)context.Resolve(typeof(string)),
            Skills = (string[])context.Resolve(typeof(string[])),
            WorkExperience = (WorkDto[])context.Resolve(typeof(WorkDto[])),
        };
    }
}
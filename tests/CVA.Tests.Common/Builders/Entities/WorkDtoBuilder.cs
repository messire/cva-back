using CVA.Application.Contracts;

namespace CVA.Tests.Common;

/// <summary>
/// A builder class that creates instances of the <see cref="WorkDto"/> type using the AutoFixture library.
/// </summary>
internal sealed class WorkDtoBuilder : ISpecimenBuilder
{
    /// <summary>
    /// A singleton instance of the <see cref="WorkDtoBuilder"/> class.
    /// </summary>
    public static readonly ISpecimenBuilder Instance = new WorkDtoBuilder();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(WorkDto)) return new NoSpecimen();

        return new WorkDto
        {
            CompanyName = (string)context.Resolve(typeof(string)),
            Role = (string)context.Resolve(typeof(string)),
            Location = (string)context.Resolve(typeof(string)),
            StartDate = (DateOnly)context.Resolve(typeof(DateOnly)),
            EndDate = (DateOnly)context.Resolve(typeof(DateOnly))
        };
    }
}
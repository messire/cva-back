using CVA.Domain.Models;

namespace CVA.Tests.Common;

/// <summary>
/// A builder class that creates instances of the <see cref="Work"/> type using the AutoFixture library.
/// </summary>
internal sealed class WorkBuilder : ISpecimenBuilder
{
    /// <summary>
    /// A singleton instance of the <see cref="WorkBuilder"/> class.
    /// </summary>
    public static readonly ISpecimenBuilder Instance = new WorkBuilder();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(Work)) return new NoSpecimen();
        
        return Work.Create(
            companyName: (string)context.Resolve(typeof(string)),
            role: (string)context.Resolve(typeof(string)),
            startDate: (DateOnly)context.Resolve(typeof(DateOnly)),
            endDate: (DateOnly)context.Resolve(typeof(DateOnly)),
            description: (string)context.Resolve(typeof(string)),
            location: (string)context.Resolve(typeof(string)),
            achievements: (string[])context.Resolve(typeof(string[])),
            techStack: ((string[])context.Resolve(typeof(string[]))).ToList()
        );
    }
}
namespace CVA.Tests.Common;

/// <summary>
/// Datetime builder.
/// </summary>
internal sealed class DateTimeBuilder
{
    /// <summary>
    /// DateTime builder instance.
    /// </summary>
    public static readonly ISpecimenBuilder Instance =
        new FilteringSpecimenBuilder(new DateTimeGenerator(), new ExactTypeSpecification(typeof(DateTime)));

    private sealed class DateTimeGenerator : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
            => request is Type type && type == typeof(DateTime)
                ? DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                : new NoSpecimen();
    }
}

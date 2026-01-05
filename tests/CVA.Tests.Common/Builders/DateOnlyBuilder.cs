namespace CVA.Tests.Common;

/// <summary>
/// DateOnly builder.
/// </summary>
internal sealed class DateOnlyBuilder
{
    /// <summary>
    /// DateOnly builder instance.
    /// </summary>
    public static readonly ISpecimenBuilder Instance =
        new FilteringSpecimenBuilder(new DateOnlyGenerator(), new ExactTypeSpecification(typeof(DateOnly)));

    private sealed class DateOnlyGenerator : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is Type type && type == typeof(DateOnly))
            {
                return DateOnly.FromDateTime(DateTime.Today);
            }

            return new NoSpecimen();
        }
    }
}

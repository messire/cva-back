using System.Reflection;
using static System.StringComparison;

namespace CVA.Tests.Common;

/// <summary>
/// A custom specimen builder for creating <see cref="PhoneNumber"/> instances.
/// </summary>
public sealed class PhoneNumberBuilder : ISpecimenBuilder
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="PhoneNumberBuilder"/> class.
    /// </summary>
    public static PhoneNumberBuilder Instance => new();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        var isEmailRequest = request switch
        {
            PropertyInfo pi => pi.PropertyType == typeof(PhoneNumber) && pi.Name.Equals("phone", OrdinalIgnoreCase),
            ParameterInfo pai => pai.ParameterType == typeof(PhoneNumber) && pai.Name!.Equals("phone", OrdinalIgnoreCase),
            Type t => t == typeof(PhoneNumber),
            _ => false
        };

        var phone = new System.Text.StringBuilder("+");
        phone.Append(Random.Shared.Next(1, 9))
            .Append(Random.Shared.Next(900, 1000))
            .Append(Random.Shared.Next(100, 1000))
            .Append(Random.Shared.Next(10, 100))
            .Append(Random.Shared.Next(10, 100));
        return isEmailRequest
            ? PhoneNumber.From(phone.ToString())
            : new NoSpecimen();
    }
}
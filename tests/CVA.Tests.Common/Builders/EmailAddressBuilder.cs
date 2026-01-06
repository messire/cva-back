using System.Reflection;
using CVA.Domain.Models;
using static System.StringComparison;

namespace CVA.Tests.Common;

/// <summary>
/// A custom specimen builder for creating <see cref="EmailAddress"/> instances.
/// Ensures that generated email addresses are valid according to domain rules.
/// </summary>
public sealed class EmailAddressBuilder : ISpecimenBuilder
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="EmailAddressBuilder"/> class.
    /// </summary>
    public static EmailAddressBuilder Instance => new();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        var isEmailRequest = request switch
        {
            PropertyInfo pi => pi.PropertyType == typeof(EmailAddress) && pi.Name.Equals("email", OrdinalIgnoreCase),
            ParameterInfo pai => pai.ParameterType == typeof(EmailAddress) && pai.Name.Equals("email", OrdinalIgnoreCase),
            Type t => t == typeof(EmailAddress),
            _ => false
        };

        return isEmailRequest
            ? EmailAddress.From($"{context.Create<string>()}@{context.Create<string>()}.test")
            : new NoSpecimen();
    }
}
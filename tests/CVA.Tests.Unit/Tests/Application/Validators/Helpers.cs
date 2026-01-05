namespace CVA.Tests.Unit.Application.Validators;

/// <summary>
/// Provides utility methods for validation testing, enabling simplified assertion of validation results in unit tests.
/// </summary>
internal static class Helpers
{
    /// <summary>
    /// Asserts whether a validation result contains an error for a specific property.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="result">The validation result to be checked.</param>
    /// <param name="expression">The expression representing the property being validated.</param>
    /// <param name="shouldHaveError">
    /// A boolean indicating whether the validation result is expected to have an error
    /// for the specified property. Pass <c>true</c> to assert the presence of an error,
    /// and <c>false</c> to assert its absence.
    /// </param>
    public static void AssertValidation<T, TProperty>(TestValidationResult<T> result, Expression<Func<T, TProperty>> expression, bool shouldHaveError)
    {
        if (shouldHaveError)
        {
            result.ShouldHaveValidationErrorFor(expression);
            return;
        }

        result.ShouldNotHaveValidationErrorFor(expression);
    }

}
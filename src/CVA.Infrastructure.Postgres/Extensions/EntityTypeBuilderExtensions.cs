using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CVA.Infrastructure.Postgres;

/// <summary>
/// Extension methods for configuring entity properties.
/// </summary>
public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Configures a property with snake_case column name and optional maximum length.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    /// <param name="propertyExpression">Expression representing the property.</param>
    /// <param name="maxLength">Optional maximum length for the property.</param>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <returns>Property builder for further configuration.</returns>
    public static PropertyBuilder<TProperty> SetProperty<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        int? maxLength = null)
        where TEntity : class
        => builder
            .Property(propertyExpression)
            .HasColumnName(propertyExpression.GetPropertyName())
            .SetMaxLength(maxLength);

    /// <summary>
    /// Configures a property with a specified column name in snake_case format and an optional maximum length.
    /// </summary>
    /// <param name="builder">The owned navigation builder for configuring properties of the dependent type.</param>
    /// <param name="propertyExpression">An expression representing the property to configure.</param>
    /// <param name="maxLength">Optional maximum length for the property, if applicable.</param>
    /// <typeparam name="TOwner">The type of the owner entity.</typeparam>
    /// <typeparam name="TDependent">The type of the dependent entity.</typeparam>
    /// <typeparam name="TProperty">The type of the property being configured.</typeparam>
    /// <returns>Property builder for further configuration.</returns>
    public static PropertyBuilder<TProperty> SetProperty<TOwner, TDependent, TProperty>(
        this OwnedNavigationBuilder<TOwner, TDependent> builder,
        Expression<Func<TDependent, TProperty>> propertyExpression,
        int? maxLength = null)
        where TOwner : class
        where TDependent : class
        => builder.Property(propertyExpression)
            .HasColumnName(propertyExpression.GetPropertyName())
            .SetMaxLength(maxLength);

    private static string GetPropertyName<TEntity, TValue>(this Expression<Func<TEntity, TValue>> propertyExpression)
        where TEntity : class
        => propertyExpression.GetPropertyAccess().Name.ToSnakeCase();

    private static PropertyBuilder<TProperty> SetMaxLength<TProperty>(this PropertyBuilder<TProperty> propertyBuilder, int? maxLength = null)
        => maxLength.HasValue
            ? propertyBuilder.HasMaxLength(maxLength.Value)
            : propertyBuilder;
}
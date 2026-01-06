using AutoFixture;
using CVA.Domain.Models;
using CVA.Tests.Common;

namespace CVA.Tests.Integration.Fixtures;

/// <summary>
/// Utility class for generating test data using AutoFixture.
/// </summary>
internal static class DataGenerator
{
    private static readonly IFixture Fixture = new Fixture().Customize(new ApplicationTestCustomization());

    /// <summary>
    /// Creates a new <see cref="User"/> instance with random data.
    /// </summary>
    /// <returns>A populated user object.</returns>
    public static User CreateUser() => Fixture.Create<User>();

    /// <summary>
    /// Creates a list of <see cref="User"/> instances.
    /// </summary>
    /// <param name="count">Number of users to create.</param>
    /// <returns>A list of populated user objects.</returns>
    public static List<User> CreateUsers(int count = 3) => Fixture.CreateMany<User>(count).ToList();

    /// <summary>
    /// Creates a new <see cref="DeveloperProfile"/> instance with random data.
    /// </summary>
    /// <returns>A populated developer profile object.</returns>
    public static DeveloperProfile CreateDeveloperProfile() => Fixture.Create<DeveloperProfile>();

    /// <summary>
    /// Creates a list of <see cref="DeveloperProfile"/> instances.
    /// </summary>
    /// <param name="count">Number of profiles to create.</param>
    /// <returns>A list of populated developer profile objects.</returns>
    public static List<DeveloperProfile> CreateDeveloperProfiles(int count = 3) => Fixture.CreateMany<DeveloperProfile>(count).ToList();

    /// <summary>
    /// Creates a random string.
    /// </summary>
    /// <returns>A random string.</returns>
    public static string CreateString() => Fixture.Create<string>();
}
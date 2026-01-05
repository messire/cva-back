using AutoFixture;
using CVA.Domain.Models;
using CVA.Tests.Common;

namespace CVA.Tests.Integration.Fixtures;

internal static class DataGenerator
{
    private static readonly IFixture Fixture = new Fixture().Customize(new ApplicationTestCustomization());

    public static User CreateUser() => Fixture.Create<User>();
    
    public static List<User> CreateUsers(int count = 3) => Fixture.CreateMany<User>(count).ToList();

    public static string CreateString() => Fixture.Create<string>();
}
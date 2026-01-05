using CVA.Domain.Models;

namespace CVA.Tests.Common;

/// <summary>
/// A builder class that creates instances of the <see cref="User"/> type using the AutoFixture library.
/// </summary>
internal sealed class UserBuilder : ISpecimenBuilder
{
    /// <summary>
    /// A singleton instance of the <see cref="UserBuilder"/> class.
    /// </summary>
    public static readonly ISpecimenBuilder Instance = new UserBuilder();

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (request is not Type type || type != typeof(User)) return new NoSpecimen();

        var name = context.Resolve(new SeededRequest(typeof(string), nameof(User.Name))).ToString()?.Split('-')[0];
        var surname = context.Resolve(new SeededRequest(typeof(string), nameof(User.Surname))).ToString()?.Split('-')[0];
        var randomNumber = Math.Abs((long)context.Resolve(typeof(long))).ToString();
        var phone = "+" + (randomNumber.Length > 11 ? randomNumber[..11] : randomNumber);
        var birthday = (DateOnly?)context.Resolve(typeof(DateOnly?));
        var summaryInfo = (string)context.Resolve(typeof(string));
        var skills = ((string[])context.Resolve(typeof(string[]))).ToList();
        var workList = ((Work[])context.Resolve(typeof(Work[]))).ToList();
        var user  = User.Create(name ?? nameof(User.Name), surname ?? nameof(User.Surname), $"{name}.{surname}@test.test".ToLower());

        user.UpdateProfile(phone, birthday, summaryInfo);
        user.ReplaceSkills(skills);
        user.ReplaceWorkExperience(workList);
        return user;
    }
}
namespace CVA.Infrastructure.Postgres;

internal static class UserPostgresMappingExtensions
{
    public static UserEntity ToEntity(this User user)
        => new()
        {
            Id = user.Id,
            GoogleSubject =  user.GoogleSubject,
            Email = user.Email.Value,
            Role = nameof(user.Role),
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        };

    public static User ToDomain(this UserEntity entity)
        => User.FromPersistence(
            id: entity.Id,
            email: entity.Email,
            googleSubject: entity.GoogleSubject,
            role: Enum.TryParse(entity.Role, true, out UserRole role) ? role : UserRole.User,
            createdAt: entity.CreatedAt,
            updatedAt: entity.UpdatedAt
        );
}
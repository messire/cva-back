namespace CVA.Infrastructure.Mongo;

/// <summary>
/// Represents a repository for managing users stored in a MongoDB database.
/// Implements the <see cref="IUserRepository"/> interface to provide methods for CRUD operations on user data.
/// </summary>
internal class UserMongoRepository(IMongoClient client, MongoOptions options) : IUserRepository
{
    private readonly IMongoCollection<UserDocument> _users = client
        .GetDatabase(options.DatabaseName)
        .GetCollection<UserDocument>("users");

    /// <inheritdoc />
    public async Task<User?> CreateAsync(User user, CancellationToken ct)
    {
        var userDocument = user.ToDocument();
        await _users.InsertOneAsync(userDocument, cancellationToken: ct);
        return user;
    }

    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var userDocument = await _users.Find(document => document.Id == id).FirstOrDefaultAsync(ct);
        return userDocument?.ToDomain();
    }

    public async Task<User?> GetByGoogleSubjectAsync(string googleSubject, CancellationToken ct)
    {
        var userDocument = await _users.Find(document => document.GoogleSubject == googleSubject).FirstOrDefaultAsync(ct);
        return userDocument?.ToDomain();
    }

    public async Task<User?> UpdateRoleAsync(Guid id, string role, CancellationToken ct)
    {
        var userDocument = await _users.Find(document => document.Id == id).FirstOrDefaultAsync(ct);
        userDocument.Role = role;
        var options = new FindOneAndReplaceOptions<UserDocument> { ReturnDocument = ReturnDocument.After };
        var user = await _users.FindOneAndReplaceAsync(document => document.Id == userDocument.Id, userDocument, options, ct);
        return user?.ToDomain();
    }
}
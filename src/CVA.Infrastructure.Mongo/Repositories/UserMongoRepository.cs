using CVA.Infrastructure.Mongo.Documents;
using CVA.Infrastructure.Mongo.Mapping;

namespace CVA.Infrastructure.Mongo;

/// <summary>
/// Provides an implementation of the <see cref="IUserRepository"/> interface for interacting with user data in a PostgreSQL database.
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

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct)
    {
        var documents = await _users.Find(_ => true).ToListAsync(ct);
        return documents.Select(document => document.ToDomain());
    }

    /// <inheritdoc />
    public async Task<User?> UpdateAsync(User updatedUser, CancellationToken ct)
    {
        var userDocument = updatedUser.ToDocument();
        var options = new FindOneAndReplaceOptions<UserDocument> { ReturnDocument = ReturnDocument.After };
        var updatedDocument = await _users.FindOneAndReplaceAsync(document => document.Id == userDocument.Id, userDocument, options, ct);
        return updatedDocument?.ToDomain();
    }

    /// <inheritdoc />
    public async Task<User?> DeleteAsync(Guid id, CancellationToken ct)
    {
        var deletedDocument = await _users.FindOneAndDeleteAsync(document => document.Id == id, cancellationToken: ct);
        return deletedDocument?.ToDomain();
    }
}
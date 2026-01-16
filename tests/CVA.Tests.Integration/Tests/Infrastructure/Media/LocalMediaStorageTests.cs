using CVA.Infrastructure.Common.Media;
using Microsoft.Extensions.Options;

namespace CVA.Tests.Integration.Tests.Infrastructure.Media;

/// <summary>
/// Integration tests for <see cref="LocalMediaStorage"/> using real file system.
/// </summary>
[Trait(Layer.Infrastructure, Category.Services)]
public sealed class LocalMediaStorageTests
{
    private const string PngContentType = "image/png";

    /// <summary>
    /// Purpose: Verify saved file is created with exact content.
    /// When: Saving an avatar to local storage.
    /// Should: Persist bytes on disk with the same content.
    /// </summary>
    [Fact]
    public async Task SaveAvatarAsync_ShouldCreateFileWithExpectedContent()
    {
        // Arrange
        var root = CreateTempRoot();
        try
        {
            var storage = CreateStorage(root);
            var expectedBytes = new byte[] { 10, 20, 30, 40, 50 };
            await using var content = new MemoryStream(expectedBytes);

            // Act
            var relativePath = await storage.SaveAvatarAsync(Guid.NewGuid(), content, PngContentType, CancellationToken.None);
            var fullPath = ToFullPath(root, relativePath);

            // Assert
            Assert.True(File.Exists(fullPath));
            var savedBytes = await File.ReadAllBytesAsync(fullPath);
            Assert.Equal(expectedBytes, savedBytes);
        }
        finally
        {
            CleanupTempRoot(root);
        }
    }

    /// <summary>
    /// Purpose: Verify directories are created automatically.
    /// When: Saving a project image with nested path.
    /// Should: Create the directory tree under the root.
    /// </summary>
    [Fact]
    public async Task SaveProjectImageAsync_ShouldCreateDirectories()
    {
        // Arrange
        var root = CreateTempRoot();
        try
        {
            var storage = CreateStorage(root);
            var userId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            await using var content = new MemoryStream(new byte[] { 1, 2, 3 });

            // Act
            var relativePath = await storage.SaveProjectImageAsync(userId, projectId, content, PngContentType, CancellationToken.None);
            var fullPath = ToFullPath(root, relativePath);
            var expectedDirectory = Path.Combine(root, "projects", userId.ToString("D"), projectId.ToString("D"));

            // Assert
            Assert.True(File.Exists(fullPath));
            Assert.True(Directory.Exists(expectedDirectory));
        }
        finally
        {
            CleanupTempRoot(root);
        }
    }

    /// <summary>
    /// Purpose: Verify saved file can be read from disk.
    /// When: Reading a file created by local storage.
    /// Should: Return the same bytes that were saved.
    /// </summary>
    [Fact]
    public async Task SaveAvatarAsync_ShouldAllowReadingExistingFile()
    {
        // Arrange
        var root = CreateTempRoot();
        try
        {
            var storage = CreateStorage(root);
            var expectedBytes = new byte[] { 7, 8, 9, 10 };
            await using var content = new MemoryStream(expectedBytes);

            // Act
            var relativePath = await storage.SaveAvatarAsync(Guid.NewGuid(), content, PngContentType, CancellationToken.None);
            var fullPath = ToFullPath(root, relativePath);

            await using var fileStream = File.OpenRead(fullPath);
            using var buffer = new MemoryStream();
            await fileStream.CopyToAsync(buffer);

            // Assert
            Assert.Equal(expectedBytes, buffer.ToArray());
        }
        finally
        {
            CleanupTempRoot(root);
        }
    }

    /// <summary>
    /// Purpose: Verify deletion removes the file from disk.
    /// When: Deleting a previously saved file.
    /// Should: Remove the file if it exists.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldRemoveFile()
    {
        // Arrange
        var root = CreateTempRoot();
        try
        {
            var storage = CreateStorage(root);
            await using var content = new MemoryStream(new byte[] { 5, 4, 3, 2, 1 });
            var relativePath = await storage.SaveAvatarAsync(Guid.NewGuid(), content, PngContentType, CancellationToken.None);
            var fullPath = ToFullPath(root, relativePath);

            // Act
            await storage.DeleteAsync(relativePath, CancellationToken.None);

            // Assert
            Assert.False(File.Exists(fullPath));
        }
        finally
        {
            CleanupTempRoot(root);
        }
    }

    /// <summary>
    /// Purpose: Verify deletion is safe when file is missing.
    /// When: Deleting a non-existing file.
    /// Should: Not throw any exception.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_ShouldNotThrow_WhenFileDoesNotExist()
    {
        // Arrange
        var root = CreateTempRoot();
        try
        {
            var storage = CreateStorage(root);
            var missingPath = "avatars/missing/avatar.png";

            // Act
            var exception = await Record.ExceptionAsync(() => storage.DeleteAsync(missingPath, CancellationToken.None));

            // Assert
            Assert.Null(exception);
        }
        finally
        {
            CleanupTempRoot(root);
        }
    }

    /// <summary>
    /// Purpose: Verify errors are surfaced for invalid root path.
    /// When: Saving into an unavailable path.
    /// Should: Throw an exception.
    /// </summary>
    [Fact]
    public async Task SaveAvatarAsync_ShouldThrow_WhenRootPathInvalid()
    {
        // Arrange
        var invalidRoot = Path.Combine(Path.GetTempPath(), "cva<>invalid");
        var storage = CreateStorage(invalidRoot);
        await using var content = new MemoryStream(new byte[] { 1, 2, 3 });

        // Act
        var exception = await Record.ExceptionAsync(() =>
            storage.SaveAvatarAsync(Guid.NewGuid(), content, PngContentType, CancellationToken.None));

        // Assert
        Assert.NotNull(exception);
    }

    private static LocalMediaStorage CreateStorage(string root)
        => new(Options.Create(new MediaOptions { RootPath = root }));

    private static string CreateTempRoot()
    {
        var root = Path.Combine(Path.GetTempPath(), "cva-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        return root;
    }

    private static void CleanupTempRoot(string root)
    {
        if (Directory.Exists(root))
        {
            Directory.Delete(root, true);
        }
    }

    private static string ToFullPath(string root, string relativePath)
        => Path.Combine(root, relativePath.Replace('/', Path.DirectorySeparatorChar));
}

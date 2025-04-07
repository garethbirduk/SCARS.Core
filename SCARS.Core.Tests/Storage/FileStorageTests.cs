using SCARS.Storage;
using System.Text.Json;

namespace SCARS.Tests.Storage;

public class FileStorageTests
{
    [Fact]
    public void FileStorage_ShouldCreateDirectory_WhenDirectoryDoesNotExist()
    {
        // Arrange
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString());  // Temporary GUID folder name
        var filepath = Path.Combine(directoryPath, $"{Guid.NewGuid()}.json");  // Create a file path inside the new directory

        // Ensure the directory does not exist before the test
        if (Directory.Exists(directoryPath))
        {
            Directory.Delete(directoryPath, true); // Clean up, delete the directory if it exists
        }

        // Act
        var storage = new FileStorage<List<string>>(new MemoryStorage<List<string>>(), filepath);

        // Assert
        // Ensure the directory is created
        Assert.True(Directory.Exists(directoryPath), $"The directory '{directoryPath}' was not created.");

        // Clean up: Delete the directory after the test
        Directory.Delete(directoryPath, true);
    }

    [Fact]
    public async Task PersistAsync_ShouldCreateFile_WhenFileDoesNotExist()
    {
        // Arrange
        // Generate a unique temporary file path for this test
        var filepath = $"{Guid.NewGuid()}.json";  // Temporary file in the current directory

        // Ensure proper cleanup by using a try-finally block
        try
        {
            // Create the FileStorage instance
            var storage = new FileStorage<List<string>>(new MemoryStorage<List<string>>(), filepath);
            var testData = new List<string> { "apple", "banana", "cherry" };

            // Act
            await storage.StoreDataAsync(testData);
            await storage.PersistAsync(); // Should create the file if it doesn't exist

            // Assert
            Assert.True(File.Exists(filepath)); // Ensure the file is created

            var fileContent = File.ReadAllText(filepath);
            var deserializedData = JsonSerializer.Deserialize<List<string>>(fileContent);

            Assert.Equal(testData, deserializedData); // Ensure the data in the file matches what was stored
        }
        finally
        {
            // Clean up: Delete the file after the test
            if (File.Exists(filepath))
            {
                File.Delete(filepath); // Ensure the file is deleted to clean up the environment
            }
        }
    }

    [Fact]
    public async Task PersistAsync_ShouldPersistData_WhenFileExists()
    {
        // Arrange
        // Generate a unique file path for this test
        var filepath = $"{Guid.NewGuid()}.json";  // Temporary file in the current directory

        // Ensure proper cleanup by using a try-finally block
        try
        {
            // Create the file and write initial data to it
            var initialData = new List<string> { "oldData" };
            var storage = new FileStorage<List<string>>(new MemoryStorage<List<string>>(), filepath);
            await storage.StoreDataAsync(initialData);
            await storage.PersistAsync(); // Write initial data to the file

            // New data to be persisted
            var updatedData = new List<string> { "apple", "banana", "cherry" };

            // Act: Persist the updated data
            await storage.StoreDataAsync(updatedData);
            await storage.PersistAsync(); // Should overwrite the file with the new data

            // Assert: Ensure the file content is updated with the new data
            var fileContent = File.ReadAllText(filepath);
            var deserializedData = JsonSerializer.Deserialize<List<string>>(fileContent);

            Assert.Equal(updatedData, deserializedData); // Ensure the data in the file matches what was stored
        }
        finally
        {
            // Clean up: Delete the file after the test
            if (File.Exists(filepath))
            {
                File.Delete(filepath); // Delete the file to clean up
            }
        }
    }

    [Fact]
    public async Task PersistAsync_ShouldThrowException_WhenFileIsReadOnly()
    {
        // Arrange
        // Generate a unique filename for this test
        var filepath = $"{Guid.NewGuid()}.json"; // Temporary file in the current directory

        // Ensure proper cleanup by using a try-finally block
        try
        {
            // Create the FileStorage instance with a MemoryStorage
            var storage = new FileStorage<List<string>>(new MemoryStorage<List<string>>(), filepath);
            var testData = new List<string> { "apple", "banana", "cherry" };

            // Create the file and write initial data to it
            await storage.StoreDataAsync(testData);
            await storage.PersistAsync(); // Write initial data to the file

            // Set the file to read-only
            File.SetAttributes(filepath, FileAttributes.ReadOnly);

            // Act & Assert: Attempting to persist again should throw an UnauthorizedAccessException
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => storage.PersistAsync());

            // Assert: The exception message should indicate access is denied
            Assert.Equal($"Access to the path '{Path.GetFullPath(filepath)}' is denied.", exception.Message);
        }
        finally
        {
            // Clean up: Reset the file attributes and delete the file after the test
            if (File.Exists(filepath))
            {
                File.SetAttributes(filepath, FileAttributes.Normal); // Reset to normal attributes
                File.Delete(filepath); // Delete the file
            }
        }
    }

    [Fact]
    public async Task PersistAsync_ShouldThrowException_WhenFolderIsReadOnly()
    {
        // Arrange
        // Generate a unique folder name and file path for this test
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString());
        var filepath = Path.Combine(folderPath, $"{Guid.NewGuid()}.json");

        // Ensure proper cleanup by using a try-finally block
        try
        {
            // Create the folder
            Directory.CreateDirectory(folderPath);

            // Set the folder to read-only
            File.SetAttributes(folderPath, FileAttributes.ReadOnly);

            // Act & Assert: Attempting to create the FileStorage instance should throw an UnauthorizedAccessException
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                Task.Run(() => new FileStorage<List<string>>(new MemoryStorage<List<string>>(), filepath))
            );

            // Assert: The exception message should indicate that access is denied to the folder
            Assert.Equal($"Write access to the path '{Path.GetDirectoryName(Path.GetFullPath(filepath))}' is denied.", exception.Message);
        }
        finally
        {
            // Clean up: Reset folder attributes to writable and delete the folder and file
            if (Directory.Exists(folderPath))
            {
                File.SetAttributes(folderPath, FileAttributes.Normal); // Reset the folder to writable
                Directory.Delete(folderPath, recursive: true); // Delete the folder and its contents
            }
        }
    }
}
using SCARS.Storage;
using SCARS.Storage.Extensions;
using SCARS.Storage.Interfaces;

namespace SCARS.Tests.Storage;

public class StorageEquivalencyTests
{
    private IDataStorage<List<string>> CreateStorage(Type type, string seedFilepath, string writeFilepath)
    {
        var memory = new MemoryStorage<List<string>>().SeedFrom(seedFilepath);

        if (type == typeof(MemoryStorage<List<string>>))
        {
            return memory;
        }
        if (type == typeof(FileStorage<List<string>>))
        {
            ResetDirectory("TestData");
            var fileStorage = new FileStorage<List<string>>(memory, writeFilepath);
            return fileStorage;
        }

        throw new NotSupportedException($"Storage type {type.Name} is not supported.");
    }

    internal static void ResetDirectory(string path)
    {
        if (Directory.Exists(path))
            Directory.Delete(path, recursive: true);

        Directory.CreateDirectory(path);
    }

    [Theory]
    [InlineData(typeof(MemoryStorage<List<string>>))]
    [InlineData(typeof(FileStorage<List<string>>))]
    public async Task StorageImplementations_ShouldBehaveTheSame(Type storageType)
    {
        var testData = new List<string> { "apple", "banana", "cherry" };

        // Dynamically construct the instance
        var storage = CreateStorage(storageType, "", Path.Combine("TestData", "FileStorageTest.json"));

        // Ensure clean slate
        await storage.ClearDataAsync();

        await storage.StoreDataAsync(testData);
        var retrieved = await storage.RetrieveDataAsync();

        Assert.Equal(testData, retrieved);

        await storage.ClearDataAsync();
        var afterClear = await storage.RetrieveDataAsync();
        Assert.Null(afterClear); // or default check
    }
}
using SCARS.Storage;
using SCARS.Storage.Extensions;

namespace SCARS.Tests.Storage;

public class DataStorageSeedingExtensionsTests
{
    private class MyType
    {
        public string? Name { get; set; }
    }

    [Fact]
    public async Task SeedFrom_AnotherStorage_CopiesData()
    {
        // Arrange
        var source = new MemoryStorage<List<string>>();
        var target = new MemoryStorage<List<string>>();
        var original = new List<string> { "x", "y", "z" };

        await source.StoreDataAsync(original);

        // Act
        target.SeedFrom(source);

        // Assert
        var result = await target.RetrieveDataAsync();
        Assert.Equal(original, result);
    }

    [Fact]
    public async Task SeedFrom_DeserializationError()
    {
        var memory = new MemoryStorage<MyType>().SeedFrom(FixturePathHelper.GetFixtureFilePath("seed.jpg", Path.Combine("Fixtures", "Storage")));
        var data = await memory.RetrieveDataAsync();
        Assert.NotNull(data);
        Assert.IsType<MyType>(data);
        Assert.Null(data.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("missing.json")]
    public async Task SeedFrom_FilePathMissing(string? filename)
    {
        var memory = new MemoryStorage<MyType>().SeedFrom(filename);
        var data = await memory.RetrieveDataAsync();
        Assert.NotNull(data);
        Assert.IsType<MyType>(data);
        Assert.Null(data.Name);
    }

    [Fact]
    public async Task SeedFrom_FilePathOk()
    {
        var memory = new MemoryStorage<MyType>().SeedFrom(FixturePathHelper.GetFixtureFilePath("seed.json", Path.Combine("Fixtures", "Storage")));
        var data = await memory.RetrieveDataAsync();
        Assert.NotNull(data);
        Assert.IsType<MyType>(data);
        Assert.Equal("Bob", data.Name);
    }

    [Fact]
    public async Task SeedWith_NullInput_ShouldSeedWithEmptyList()
    {
        // Arrange
        var memory = new MemoryStorage<List<string>>();

        // Act
        var seeded = memory.SeedWith(null);

        // Assert
        var result = await seeded.RetrieveDataAsync();
        Assert.NotNull(result); // Because new List<string>() should be created
        Assert.Empty(result);   // Because it’s a new instance
    }

    [Fact]
    public async Task SeedWith_ShouldStoreProvidedData()
    {
        // Arrange
        var memory = new MemoryStorage<List<string>>();
        var data = new List<string> { "one", "two" };

        // Act
        var seeded = memory.SeedWith(data);

        // Assert
        var result = await seeded.RetrieveDataAsync();
        Assert.Equal(data, result);
    }
}
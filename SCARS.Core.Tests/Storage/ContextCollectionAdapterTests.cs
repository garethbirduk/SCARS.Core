using SCARS.Storage;

namespace SCARS.Tests.Storage;

public class ContextCollectionAdapterTests
{
    private ContextCollectionAdapter<FakeContext, string> CreateAdapter(out MemoryStorage<FakeContext> memory)
    {
        memory = new MemoryStorage<FakeContext>();
        memory.StoreDataAsync(new FakeContext()).Wait();

        return new ContextCollectionAdapter<FakeContext, string>(
            memory,
            ctx => ctx.Items
        );
    }

    [Fact]
    public async Task AddAsync_AddsItemToCollection()
    {
        var adapter = CreateAdapter(out var memory);

        await adapter.AddAsync("apple");

        var context = await memory.RetrieveDataAsync();
        Assert.Contains("apple", context.Items);
    }

    [Fact]
    public async Task ClearAsync_RemovesAllItems()
    {
        var adapter = CreateAdapter(out var memory);
        memory.Data.Items.AddRange(new[] { "x", "y", "z" });

        await adapter.ClearAsync();

        var result = await adapter.GetAllAsync();
        Assert.Empty(result);
    }

    [Fact]
    public void Constructor_ThrowsIfNullsPassed()
    {
        var mem = new MemoryStorage<FakeContext>();

        Assert.Throws<ArgumentNullException>(() =>
            new ContextCollectionAdapter<FakeContext, string>(null!, ctx => ctx.Items));

        Assert.Throws<ArgumentNullException>(() =>
            new ContextCollectionAdapter<FakeContext, string>(mem, null!));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsItems()
    {
        var adapter = CreateAdapter(out var memory);
        memory.Data.Items.AddRange(new[] { "a", "b", "c" });

        var result = await adapter.GetAllAsync();

        Assert.Equal(3, result.Count);
        Assert.Contains("b", result);
    }

    [Fact]
    public async Task RemoveAsync_RemovesMatchingItems()
    {
        var adapter = CreateAdapter(out var memory);
        memory.Data.Items.AddRange(new[] { "one", "two", "three" });

        await adapter.RemoveAsync(x => x.Contains("o"));

        var remaining = await adapter.GetAllAsync();
        Assert.Equal(new[] { "three" }, remaining);
    }

    public class FakeContext
    {
        public List<string> Items { get; set; } = new();
    }
}
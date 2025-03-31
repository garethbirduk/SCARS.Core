using SCARS.Storage.Interfaces;

namespace SCARS.Storage;

/// <summary>
/// Adapts a collection within a context object to behave like a standalone ICollectionDataStorage.
/// </summary>
public class ContextCollectionAdapter<TContext, TItem> : ICollectionDataStorage<TItem>
    where TContext : class
{
    private readonly Func<TContext, IList<TItem>> _collectionSelector;
    private readonly IDataStorage<TContext> _contextStorage;

    public ContextCollectionAdapter(
        IDataStorage<TContext> contextStorage,
        Func<TContext, IList<TItem>> collectionSelector)
    {
        _contextStorage = contextStorage ?? throw new ArgumentNullException(nameof(contextStorage));
        _collectionSelector = collectionSelector ?? throw new ArgumentNullException(nameof(collectionSelector));
    }

    public async Task AddAsync(TItem item)
    {
        var context = await _contextStorage.RetrieveDataAsync();
        _collectionSelector(context).Add(item);
        await _contextStorage.StoreDataAsync(context);
    }

    public async Task ClearAsync()
    {
        var context = await _contextStorage.RetrieveDataAsync();
        _collectionSelector(context).Clear();
        await _contextStorage.StoreDataAsync(context);
    }

    public async Task<IReadOnlyCollection<TItem>> GetAllAsync()
    {
        var context = await _contextStorage.RetrieveDataAsync();
        return _collectionSelector(context).ToList();
    }

    public async Task RemoveAsync(Predicate<TItem> match)
    {
        var context = await _contextStorage.RetrieveDataAsync();
        var collection = _collectionSelector(context);
        var itemsToRemove = collection.Where(x => match(x)).ToList();
        foreach (var item in itemsToRemove)
        {
            collection.Remove(item);
        }
        await _contextStorage.StoreDataAsync(context);
    }
}
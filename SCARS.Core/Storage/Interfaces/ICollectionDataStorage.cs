namespace SCARS.Storage.Interfaces;

/// <summary>
/// A generic interface for persisting collection data storage.
/// </summary>
/// <typeparam name="T">The collection type</typeparam>
[SCARS.Attributes.Unmockable("Use MemoryStorage or FileStorage instead.")]
public interface ICollectionDataStorage<T>
{
    Task AddAsync(T item);

    Task ClearAsync();

    Task<IReadOnlyCollection<T>> GetAllAsync();

    Task RemoveAsync(Predicate<T> match);
}
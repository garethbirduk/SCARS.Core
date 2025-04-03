namespace SCARS.Storage.Interfaces;

/// <summary>
/// A generic interface for data storage for an entire data context.
/// </summary>
[SCARS.Attributes.Unmockable("Use MemoryStorage or FileStorage instead.")]
public interface IDataStorage<T>
    where T : class
{
    /// <summary>
    /// Clears the data context.
    /// </summary>
    Task ClearDataAsync();

    /// <summary>
    /// Retrieves the entire data context.
    /// </summary>
    Task<T> RetrieveDataAsync();

    /// <summary>
    /// Stores the entire data context.
    /// </summary>
    Task StoreDataAsync(T data);
}
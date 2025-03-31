namespace SCARS.Storage.Interfaces;

/// <summary>
/// A generic interface for persisting data storage.
/// </summary>
[SCARS.Attributes.Unmockable("Use MemoryStorage or FileStorage instead.")]
public interface IDataPersistence<T>
    where T : class
{
    /// <summary>
    /// Flush any pending changes to persistent storage
    /// </summary>
    /// <returns></returns>
    Task PersistAsync();
}
using SCARS.Storage.Interfaces;

namespace SCARS.Storage;

/// <summary>
/// In-memory implementation of IDataStorage for testing or runtime-only usage.
/// </summary>
public class MemoryStorage<T> : IDataStorage<T>
    where T : class
{
    /// <summary>
    /// Gets or sets the in-memory data
    /// </summary>
    public T Data { get; protected set; }

    /// <inheritdoc/>
    public Task ClearDataAsync()
    {
        Data = default!;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<T> RetrieveDataAsync()
    {
        return Task.FromResult(Data);
    }

    /// <inheritdoc/>
    public Task StoreDataAsync(T data)
    {
        Data = data;
        return Task.CompletedTask;
    }
}
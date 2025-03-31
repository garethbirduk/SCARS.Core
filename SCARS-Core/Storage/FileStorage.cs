using SCARS.Storage.Interfaces;
using System.Text.Json;

namespace SCARS.Storage;

public class FileStorage<T> : IDataStorage<T>, IDataPersistence<T>
    where T : class
{
    private readonly IDataStorage<T> _inner;
    private readonly string _writePath;

    public FileStorage(IDataStorage<T> inner, string writeFilePath)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        _writePath = writeFilePath ?? throw new ArgumentNullException(nameof(writeFilePath));
    }

    public Task ClearDataAsync() => _inner.ClearDataAsync();

    public async Task PersistAsync()
    {
        var data = await _inner.RetrieveDataAsync();
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_writePath, json);
    }

    public Task<T> RetrieveDataAsync() => _inner.RetrieveDataAsync();

    public Task StoreDataAsync(T data) => _inner.StoreDataAsync(data);
}
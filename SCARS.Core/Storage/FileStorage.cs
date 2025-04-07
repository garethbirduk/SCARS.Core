using System.Diagnostics.CodeAnalysis;
using SCARS.Storage.Interfaces;
using System.Text.Json;

namespace SCARS.Storage;

public class FileStorage<T> : IDataStorage<T>, IDataPersistence<T>
    where T : class
{
    private readonly IDataStorage<T> _inner;
    private readonly string _writePath;

    public FileStorage([NotNull] IDataStorage<T> inner, [NotNull] string writeFilePath)
    {
        _inner = inner;
        _writePath = writeFilePath;

        // Ensure that the directory exists before trying to write to the file.
        var directory = Path.GetDirectoryName(_writePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var directoryAttributes = File.GetAttributes(directory);
            if (directoryAttributes.HasFlag(FileAttributes.ReadOnly))
            {
                throw new UnauthorizedAccessException($"Write access to the path '{directory}' is denied.");
            }
        }
    }

    public Task ClearDataAsync() => _inner.ClearDataAsync();

    public async Task PersistAsync()
    {
        // Ensure the file exists; if not, create an empty file
        if (!File.Exists(_writePath))
        {
            // Create an empty file (you can use File.Create, which will create the file and leave it empty)
            using (File.Create(_writePath)) { }
        }

        // Now serialize and persist the data
        var data = await _inner.RetrieveDataAsync();
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

        // Write the data to the file
        File.WriteAllText(_writePath, json);
    }

    public Task<T> RetrieveDataAsync() => _inner.RetrieveDataAsync();

    public Task StoreDataAsync(T data) => _inner.StoreDataAsync(data);
}
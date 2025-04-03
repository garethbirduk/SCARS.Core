using SCARS.Storage.Interfaces;
using System.Text.Json;

namespace SCARS.Storage.Extensions;

public static class DataStorageSeedingExtensions
{
    /// <summary>
    /// Seeds this storage from a JSON file. If the file does not exist or is null/empty, it uses a new T.
    /// </summary>
    public static IDataStorage<T> SeedFrom<T>(this IDataStorage<T> target, string? filePath)
        where T : class, new()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return target.SeedFrom(new T());

            if (File.Exists(filePath))
                return target.SeedFrom(JsonSerializer.Deserialize<T>(File.ReadAllText(filePath)));

            return target.SeedFrom(new T());
        }
        catch
        {
            return target.SeedFrom(new T());
        }
    }

    /// <summary>
    /// Seeds this storage from another IDataStorage source.
    /// </summary>
    public static IDataStorage<T> SeedFrom<T>(this IDataStorage<T> target, IDataStorage<T> source)
        where T : class
    {
        return target.SeedFrom(source.RetrieveDataAsync().GetAwaiter().GetResult());
    }

    /// <summary>
    /// Seeds this storage from a raw object.
    /// </summary>
    public static IDataStorage<T> SeedFrom<T>(this IDataStorage<T> target, T? data)
        where T : class
    {
        if (data is not null)
        {
            target.StoreDataAsync(data).GetAwaiter().GetResult();
        }

        return target;
    }

    /// <summary>
    /// Seeds this storage with existing data
    /// </summary>
    public static IDataStorage<T> SeedWith<T>(this IDataStorage<T> target, T data)
        where T : class, new()
    {
        return target.SeedFrom(data ?? new T());
    }
}
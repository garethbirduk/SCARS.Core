using SCARS.Storage.Interfaces;

namespace SCARS.Tests.Storage;

public static class DataStorageTestCases
{
    public static async Task RunCommonStorageTests<T>(IDataStorage<T> storage, T testData)
        where T : class
    {
        await storage.ClearDataAsync();

        await storage.StoreDataAsync(testData);
        var retrieved = await storage.RetrieveDataAsync();

        Assert.Equal(testData, retrieved);

        await storage.ClearDataAsync();
        var afterClear = await storage.RetrieveDataAsync();
        Assert.Null(afterClear); // or default value check
    }
}
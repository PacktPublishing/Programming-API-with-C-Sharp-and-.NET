using Azure;
using AzureTableStorage.Models;

namespace AzureTableStorage.Abstractions
{
    public interface IStorageTableService
    {
        Task<Response> UpsertEntityAsync(MasterFileTableModel entity);
    }
}

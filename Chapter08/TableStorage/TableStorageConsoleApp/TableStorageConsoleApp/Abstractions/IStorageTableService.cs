using Azure;
using TableStorageConsoleApp.Models;

namespace TableStorageConsoleApp.Abstractions
{
    public interface IStorageTableService
    {
        Task<Response> UpsertEntityAsync(TableModel entity);
    }
}

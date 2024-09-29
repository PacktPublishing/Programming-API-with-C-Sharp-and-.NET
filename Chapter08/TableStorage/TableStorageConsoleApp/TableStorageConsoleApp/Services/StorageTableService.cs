using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using TableStorageConsoleApp.Abstractions;
using TableStorageConsoleApp.Models;

namespace TableStorageConsoleApp.Services
{
    public class StorageTableService : IStorageTableService
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly Task<Response<TableItem>> _tableCreationTask;

        public StorageTableService(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient;
            _tableCreationTask = _tableServiceClient.CreateTableIfNotExistsAsync("ExceptionsTable");
        }

        public async Task<Response> UpsertEntityAsync(TableModel entity)
        {
            var response = await _tableCreationTask;
            var table = _tableServiceClient.GetTableClient(response.Value.Name);
            return await table.UpsertEntityAsync(entity);
        }
    }
}

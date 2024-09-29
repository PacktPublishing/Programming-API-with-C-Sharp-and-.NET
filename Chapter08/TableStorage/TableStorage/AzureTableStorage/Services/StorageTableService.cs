using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using AzureTableStorage.Abstractions;
using AzureTableStorage.Models;
using Microsoft.Extensions.Options;

namespace AzureTableStorage.Services
{
    public class StorageTableService : IStorageTableService
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly Task<Response<TableItem>> _tableCreationTask;

        public StorageTableService(TableServiceClient tableServiceClient, IOptions<MasterFileOptions> options)
        {
            _tableServiceClient = tableServiceClient;
            _tableCreationTask = _tableServiceClient.CreateTableIfNotExistsAsync(options.Value.TableName);
        }

        public async Task<Response> UpsertEntityAsync(MasterFileTableModel entity)
        {
            var response = await _tableCreationTask;
            var table = _tableServiceClient.GetTableClient(response.Value.Name);
            return await table.UpsertEntityAsync(entity);
        }
    }
}
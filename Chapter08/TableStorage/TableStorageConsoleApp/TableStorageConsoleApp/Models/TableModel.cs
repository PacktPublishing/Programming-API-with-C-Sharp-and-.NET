using Azure;
using Azure.Data.Tables;

namespace TableStorageConsoleApp.Models
{
    public class TableModel : ITableEntity
    {
        required public string PartitionKey { get; set; } // Exception type
        required public string RowKey { get; set; }  // TableStorageConsoleApp-2021-09-01T00:00:00.0000000Z
        public DateTimeOffset? Timestamp { get; set; }
        public string? Message { get; set; }
        public ETag ETag { get; set; } = ETag.All;
    }
}

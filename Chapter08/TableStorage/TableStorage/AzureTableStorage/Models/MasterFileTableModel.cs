using Azure;
using Azure.Data.Tables;

namespace AzureTableStorage.Models
{
    public class MasterFileTableModel : ITableEntity
    {
            required public string PartitionKey { get; set; }

            required public string RowKey { get; set; }

            public DateTimeOffset? Timestamp { get; set; }

            public ETag ETag { get; set; }

            public DateTime? StartDate { get; set; }

            public DateTime? EndDate { get; set; }

            public string? VehicleId { get; set; }

            public string? FileName { get; set; }

            public int? FileSize { get; set; }

            public string? SasUrl { get; set; }
        }
}

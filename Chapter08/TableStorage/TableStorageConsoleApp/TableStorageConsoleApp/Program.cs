using Azure.Data.Tables;
using TableStorageConsoleApp.Models;
using TableStorageConsoleApp.Services;

namespace TableStorageConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading...");
            for(; ; )
            {
                ThrowException();
            }
        }

        static void ThrowException()
        {
            try
            {
                Random rand = new Random();
                var random = rand.Next(0, 10);
                switch (random)
                {
                    case 0:
                        throw new ArgumentException("Argument Exception");
                    case 1:
                        throw new ArgumentNullException("Argument Null Exception");
                    case 2:
                        throw new ArgumentOutOfRangeException("Argument Out Of Range Exception");
                    case 3:
                        throw new DivideByZeroException("Divide By Zero Exception");
                    case 4:
                        throw new FileNotFoundException("File Not Found Exception");
                    case 5:
                        throw new FormatException("Format Exception");
                    case 6:
                        throw new IndexOutOfRangeException("Index Out Of Range Exception");
                    case 7:
                        throw new InvalidOperationException("Invalid Operation Exception");
                    case 8:
                        throw new KeyNotFoundException("Key Not Found Exception");
                    case 9:
                        throw new NotImplementedException("Not Implemented Exception");
                    case 10:
                        throw new NotSupportedException("Not Supported Exception");
                    default:
                        throw new Exception("Generic Exception - you should never see this");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TableModel entity = new TableModel
                {
                    PartitionKey = ex.GetType().Name,
                    RowKey = $"TableStorageConsoleApp-{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ")}",
                    Message = ex.Message,
                    Timestamp = DateTimeOffset.UtcNow
                };
                TableServiceClient tableServiceClient = new TableServiceClient("DefaultEndpointsProtocol=https;AccountName=<your account name>;AccountKey=<your account key>;EndpointSuffix=core.windows.net");

                var storageTableService = new StorageTableService(tableServiceClient);
                storageTableService.UpsertEntityAsync(entity).Wait();
                Thread.Sleep(2000);
            }
        }
    }
}

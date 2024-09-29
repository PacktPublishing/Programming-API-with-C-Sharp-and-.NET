using Cars.Data.Entities;

namespace Cars.Data.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> Get(
            bool returnDeletedRecords, 
            int pageNumber, 
            int pageSize);

        Task<Car?> Get(int id);
        
        Task<int> UpsertAsync(Car car);
        
        Task<int> DeleteAsync(int id);
    }
}

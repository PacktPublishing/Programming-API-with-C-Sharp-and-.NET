using Cars.Data.Entities;
using Cars.Data.Interfaces;
using Dapper;

namespace Cars.Data.Repositories;

// Resources
// https://github.com/DapperLib/Dapper
// https://github.com/DapperLib/Dapper/tree/main/Dapper.SqlBuilder
public class CarRepository : ICarRepository
{
    readonly DatabaseConnectionFactory databaseConnectionFactory;

    public CarRepository(DatabaseConnectionFactory databaseConnectionFactory)
    {
        this.databaseConnectionFactory = databaseConnectionFactory;
    }

    public async Task<IEnumerable<Car>> GetAll(bool returnDeletedRecords = false)
    {
        var builder = new SqlBuilder();
        var sqlTemplate = builder.AddTemplate("SELECT * FROM car /**where**/");
        if (!returnDeletedRecords)
        {
            builder.Where("is_deleted=0");
        }
        using var db = databaseConnectionFactory.GetConnection();
        return await db.QueryAsync<Car>(sqlTemplate.RawSql,sqlTemplate.Parameters);
    }
    
    public async Task<Car?> Get(int id)
    {
        var query = "select * from car where id=@id";
        using var db = databaseConnectionFactory.GetConnection();
        return await db.QuerySingleOrDefaultAsync<Car>(query, new {id});
    }
    
    public async Task<int> UpsertAsync(Car car)
    {
        using var db = databaseConnectionFactory.GetConnection();
        var sql = @"
        DECLARE @InsertedRows AS TABLE (Id int);
        MERGE INTO Car AS target
        USING (SELECT @Id AS Id, @Name AS Name, @Model_Year AS Model_Year, @Is_Deleted AS Is_Deleted, @Origin AS origin ) AS source 
        ON target.Id = source.Id
        WHEN MATCHED THEN 
            UPDATE SET 
                Name = source.Name, 
                Model_Year = source.Model_Year, 
                Is_Deleted = source.Is_Deleted,
                Origin = source.Origin
        WHEN NOT MATCHED THEN
            INSERT (Name, Model_Year, Is_Deleted, Origin)
            VALUES (source.Name, source.Model_Year, source.Is_Deleted, source.Origin)
            OUTPUT inserted.Id INTO @InsertedRows
        ;

        SELECT Id FROM @InsertedRows;
    ";

        var newId = await db.QuerySingleOrDefaultAsync<int>(sql, car);
        return newId == 0 ? car.Id : newId;
    }
    public async Task<int> DeleteAsync(int id)
    {
        using var db = databaseConnectionFactory.GetConnection();
        var query = "UPDATE car SET Is_Deleted = 1 WHERE Id = @Id";
        return await db.ExecuteAsync(query, new { Id = id });
    }
    
}
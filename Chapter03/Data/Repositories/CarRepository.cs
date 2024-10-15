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

    public async Task<Car?> Get(int carId)
    {
        var sql =
            $@"SELECT * 
       FROM  
            Cars C 
       WHERE 
            C.id = @{nameof(carId)} 
            AND C.is_deleted = 0";
        var param = new
        {
            carId
        };
        var car = await QueryFirstOrDefaultAsync<Car>(sql, param);

        return car;
    }

    private async Task<T> QueryFirstOrDefaultAsync<T>(string sql, params object[] param)
    {
        using var db = databaseConnectionFactory.GetConnection();
        return await db.QueryFirstOrDefaultAsync<T>(sql, param);
    }

    public async Task<IEnumerable<Car>> GetAll(
        bool returnDeletedRecords = false)
    {
        var builder = new SqlBuilder();
        var sqlTemplate = builder.AddTemplate(
            "SELECT * FROM car " +
            "/**where**/ ");
        if (!returnDeletedRecords)
        {
            builder.Where("is_deleted=0");
        }
        using var db = databaseConnectionFactory.GetConnection();
        return await db.QueryAsync<Car>(sqlTemplate.RawSql, sqlTemplate.Parameters);
    }

    public async Task<int> UpsertAsync(Car car)
    {
        using var db = databaseConnectionFactory.GetConnection();
        var sql = @"
        DECLARE @InsertedRows AS TABLE (Id int);
        MERGE INTO Car AS target
        USING (SELECT @Id AS Id, @Name AS Name, @MPG as MPG, @Cylinders as Cylinders, @Displacement as Displacement, @Horsepower as Horsepower,
        @Weight as Weight, @Acceleration as Acceleration, @Model_Year AS Model_Year, @Origin AS origin, @Is_Deleted AS Is_Deleted ) AS source 
        ON target.Id = source.Id
        WHEN MATCHED THEN 
            UPDATE SET 
                Name = source.Name, 
                MPG = source.MPG,
                Cylinders = source.Cylinders,
                Displacement = source.Displacement,
                Horsepower = source.Horsepower,
                Weight = source.Weight,
                Acceleration = source.Acceleration,
                Model_Year = source.Model_Year,
                Origin = source.Origin,
                Is_Deleted = source.Is_Deleted
        WHEN NOT MATCHED THEN
            INSERT (Name, Mpg, Cylinders, Displacement, Horsepower, Weight, Acceleration, Model_Year, Origin, Is_deleted)
            VALUES (source.Name, source.MPG, source.Cylinders, source.Displacement, source.Horsepower, source.Weight, source.Acceleration, 
            source.Model_Year, source.Origin, source.Is_Deleted)
            OUTPUT inserted.Id INTO @InsertedRows
        ;

        SELECT Id FROM @InsertedRows;
    ";

        var newId = await db.QuerySingleOrDefaultAsync<int>(sql, car);
        return newId == 0 ? car.id : newId;
    }
    public async Task<int> DeleteAsync(int id)
    {
        using var db = databaseConnectionFactory.GetConnection();
        var query = "UPDATE car SET Is_Deleted = 1 WHERE Id = @Id";
        return await db.ExecuteAsync(query, new { Id = id });
    }
    
}
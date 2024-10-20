using Cars.Data;
using Cars.Data.Interfaces;
using Cars.Data.Repositories;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAutoMapper(typeof(Program));
        // Load DB configuration and register the connection factory for injection
        var configuration = builder.Configuration;
        builder.Services.Configure<DbSettings>(configuration.GetSection("ConnectionStrings"));
        builder.Services.AddTransient<DatabaseConnectionFactory>();
        builder.Services.AddTransient<CarRepository>();
        builder.Services.RegisterDataAccessDependencies();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        //app.MapGet("/car-minimal", (ICarRepository carRepository) => { return carRepository.GetAll(); });

        app.Run();
    }
}

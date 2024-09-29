using System.Reflection;
using Cars.Data;
using Cars.Data.Interfaces;
using Cars.Data.Repositories;
using Microsoft.OpenApi.Models;

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

        builder.Services.AddSwaggerGen(
         c =>
         {
             c.SwaggerDoc(
                 "v1",
                 new OpenApiInfo
                 {
                     Title = $"{Assembly.GetExecutingAssembly().GetName().Name}",
                     Version = "Version 1",
                     Description = "Create documentation for Cars",
                     Contact = new OpenApiContact
                     {
                         Name = "Jesse Liberty",
                         Email = "jesseliberty@gmail.com",
                         Url = new Uri("https://jesseliberty.com")
                     }
                 });
             var xmlFilename = System.IO.Path.Combine(System.AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
             c.IncludeXmlComments(xmlFilename);
         });


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

        app.Run();
    }
}

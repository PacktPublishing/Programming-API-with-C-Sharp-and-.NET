using Cars.Data.Repositories;
using Cars.Services;

namespace Cars.Data.Interfaces
{
    public static class Extensions
    {
        public static IServiceCollection RegisterDataAccessDependencies(this IServiceCollection services)
        {
            services.AddTransient<ICarRepository, CarRepository>();
            services.AddTransient<ICarService, CarService>();

            return services;
        }
    }

}

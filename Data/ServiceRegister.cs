using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Data;

namespace Data
{
    public static class ServiceRegister
    {
        public static void AddDataAccessorLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"), b =>
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                ));
        }
    }
}
using Business.Hubs;
using Business.IServices;
using Business.Services;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Threading.Tasks;

namespace Business
{
    public static class ServiceRegister
    {
        public static void AddBusinessLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IAppointmentService, ClsAppointmentService>();
            //services.AddTransient<IAuthService, ClsAuthService>();
            services.AddTransient<ISearchService, ClsSearchService>();
            services.AddTransient<IManagementService, ClsManagementService>();
            services.AddTransient<IScheduleService, ScheduleService>();
            services.AddTransient<IScheduleDoctorService, ScheduleDoctorService>();
            services.AddSingleton<IDbClient, DbClient>();
            services.AddTransient<IAuthService, ClsAuthService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();
            services.AddTransient<IHealthCheckService, ClsHealthCheckService>();
        }

        public static void AddAuthenticationAuthorization(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireUppercase = false;
            })
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            services.AddAuthentication("Bearer")
               .AddJwtBearer("Bearer", options =>
               {
                   options.Authority = "https://localhost:44315/";
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = false
                   };

                   options.Events = new JwtBearerEvents()
                   {
                       OnMessageReceived = context =>
                       {
                           var accessToken = context.Request.Query["access_token"];

                           // If the request is for our hub...
                           var path = context.HttpContext.Request.Path;
                           if (!string.IsNullOrEmpty(accessToken) &&
                               (path.StartsWithSegments("/notification-hub")))
                           {
                               // Read the token out of the query string
                               context.Token = accessToken;
                           }
                           return Task.CompletedTask;
                       }
                   };
               });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    //policy.RequireRole("Admin");
                    policy.RequireClaim("role", "Admin");
                });
            });
        }
    }
}

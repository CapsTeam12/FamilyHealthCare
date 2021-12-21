using AuthService.IdentityServer;
using AuthService.Security.Authorization.Handlers;
using AuthService.Security.Authorization.Requirements;
using Contract.Constants;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace AuthService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataAccessorLayer(Configuration);

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

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
            .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
            .AddInMemoryClients(IdentityServerConfig.Clients)
            .AddAspNetIdentity<User>()
            .AddProfileService<CustomProfileService>()
            .AddDeveloperSigningCredential();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/CustomAuthentication/Login";
            });

            services.AddAuthentication()
                .AddLocalApi("Bearer", option =>
                {
                    option.ExpectedScope = CustomIdentityServerConstants.ApiScopeName;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(LocalApi.PolicyName, policy =>
                {
                    policy.AddAuthenticationSchemes("Bearer");
                    policy.RequireAuthenticatedUser();
                });

                options.AddPolicy("ADMIN_ROLE_POLICY", policy =>
                    policy.Requirements.Add(new AdminRoleRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, AdminRoleHandler>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

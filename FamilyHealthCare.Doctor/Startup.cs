using FamilyHealthCare.SharedLibrary;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Doctor
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                 .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                 {
                     options.Authority = "https://localhost:44315/";
                     options.RequireHttpsMetadata = false;
                     options.GetClaimsFromUserInfoEndpoint = true;

                     options.ClientId = "Doctor";
                     options.ClientSecret = "secret";
                     options.ResponseType = "code";

                     options.SaveTokens = true;

                     options.Scope.Add("openid");
                     options.Scope.Add("profile");
                     options.Scope.Add("familyhealthcare.api");

                     options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                     {
                         NameClaimType = "name",
                         RoleClaimType = "role"
                     };
                 });

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddCustomHttpClient(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

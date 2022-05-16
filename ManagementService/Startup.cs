using Business;
using Contract.DTOs.MailService;
using Data;
using Data.Entities;
using FluentValidation.AspNetCore;
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
using System.Reflection;
using System.Threading.Tasks;

namespace ManagementService
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
            {
                services.AddCors(c => {
                    c.AddPolicy("policyName", p => {
                        p.WithOrigins("https://localhost:44367").AllowAnyMethod().AllowAnyHeader();
                        p.WithOrigins("https://localhost:44369").AllowAnyMethod().AllowAnyHeader();
                        //p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
                });
            }
            services.AddOptions();
            var mailsettings = Configuration.GetSection("MailSettings"); 
            services.Configure<MailSettings>(mailsettings); 
            services.AddBusinessLayer();
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
            services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                }
            );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ManagementService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ManagementService v1"));
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("policyName");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}

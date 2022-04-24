using AutoMapper.Configuration;
using Business.IServices;
using Business.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
            services.AddTransient<IZoomService, ZoomService>();
            services.AddTransient<IParnerService, PartnerService>();
            services.AddTransient<ISendMailService, SendMailService>();
            services.AddTransient<IMedicineService, MedicineService>();
            services.AddTransient<IMedicalRecordService, MedicalRecordService>();
        }
    }
}

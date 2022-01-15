using Business;
using Contract.DTOs;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHealCare.IntegationTest.TestData
{
    class AppointmentArrangeData
    {
        public static async Task InitAppointmentDataAsync(BaseRepository<Appointment> appointmentRepository)
        {
            if (!appointmentRepository.GetAll().Result.Any())
            {
                var appointmentList = new List<Appointment>()
                {
                    //new Appointment
                    //{
                    //    Id = 1,
                    //    Time = new DateTime(01/03/2022),
                    //    Description = "Re-examination",
                    //    TherapistId = 1
                    //},
                    //new Appointment
                    //{
                    //    Id = 2,
                    //    Time = new DateTime(01/04/2022),
                    //    Description = "Re-examination",
                    //    TherapistId = 1
                    //},

                };

                //foreach (Appointment appointment in appointmentList)
                //{
                //    await appointmentRepository.Create(appointment);
                //}
            }
        }

        public static string GetAppointmentDto()
        {
            return "searching";
        }
        public static AppointmentCreateDto GetAppointmentCreateDto()
        {
            return new AppointmentCreateDto()
            {
                StartTime = new DateTime(01 / 04 / 2022),
                Description = "Re-examination",
                TherapistId = ""
            };
        }


    }
}
    

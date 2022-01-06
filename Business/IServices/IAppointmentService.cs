using Contract.DTOs;
using Contract.DTOs.AppoimentService;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IAppointmentService
    {
        //Task<IActionResult> GetAppointmentsAsync(string search);
        Task<IEnumerable<AppointmentDetailsDto>> GetAppointments(string userId);
        Task<AppointmentDetailsDto> GetAppointmentById(string id);
        Task<AppointmentDetailsDto> BookingAppointment(AppointmentCreateDto model,string userId);
        Task<AppointmentDetailsDto> RescheduleAppointment(AppointmentRescheduleDto model,string id);
        Task CancelAppointment(string id);

    }
}

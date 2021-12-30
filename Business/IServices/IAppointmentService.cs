using Contract.DTOs;
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
        Task<IEnumerable<AppointmentDetailsDto>> GetAppointments();
        Task<AppointmentDetailsDto> GetAppointmentById(string id);
        Task<AppointmentDetailsDto> RescheduleAppointment(AppointmentCreateDto model,string id);
        Task CancelAppointment(string id);

    }
}

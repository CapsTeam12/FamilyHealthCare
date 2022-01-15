using Contract.DTOs;
<<<<<<<<< Temporary merge branch 1
=========
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
        public Task<IActionResult> GetAppointmentsAsync(string search);
        public Task<IActionResult> CreateAppointmentAsync(AppointmentCreateDto appointmentCreateDto);

        //Task<IActionResult> GetAppointmentsAsync(string search);
        Task<IEnumerable<AppointmentDetailsDto>> GetAppointments(string userId);
        Task<AppointmentDetailsDto> GetAppointmentById(string id);
        Task<AppointmentDetailsDto> BookingAppointment(AppointmentCreateDto model,string userId);
        Task<AppointmentDetailsDto> RescheduleAppointment(AppointmentRescheduleDto model,string id);
        Task CancelAppointment(string id);

    }
}

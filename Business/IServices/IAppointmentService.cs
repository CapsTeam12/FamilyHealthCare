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
        Task<IEnumerable<AppointmentDetailsDto>> GetTotalAppointments();
        int GetTotalAppointmentsByPatient(string id);
        int GetTotalAppointmentsByDoctor(string id);
        public Task<IActionResult> GetAppointmentsAsync(string search);
        public Task<IActionResult> CreateAppointmentAsync(AppointmentCreateDto appointmentCreateDto);
        Task<IEnumerable<AppointmentDetailsDto>> GetAppointments(string userId);
        Task<AppointmentDetailsDto> GetAppointmentById(string id);
        Task<AppointmentDetailsDto> BookingAppointment(AppointmentCreateDto model,string userId);
        Task<AppointmentDetailsDto> RescheduleAppointment(AppointmentRescheduleDto model,string id);
        Task<AppointmentDetailsDto> CancelAppointment(string id);

    }
}

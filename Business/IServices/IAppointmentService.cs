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
        public Task<IActionResult> GetAppointmentsAsync(string search);
        public Task<IActionResult> CreateAppointmentAsync(AppointmentCreateDto appointmentCreateDto);

    }
}

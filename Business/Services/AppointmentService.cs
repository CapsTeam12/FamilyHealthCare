using AutoMapper;
using Business.IServices;
using Contract.DTOs;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AppointmentService : ControllerBase, IAppointmentService
    {
        private readonly IBaseRepository<Appointment> _repository;
        private readonly IMapper _mapper;
        public AppointmentService(IBaseRepository<Appointment> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAppointmentsAsync(string search)
        {
            var appointments = await _repository
                                .Entities
                                .Include(a => a.Therapist)
                                .Where(a => a.Description.ToLower().Contains(search.ToLower()))
                                .ToListAsync();
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDetailsDto>>(appointments);
            return Ok(appointmentDtos);
        }
    }
}

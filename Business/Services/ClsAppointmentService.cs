using AutoMapper;
using Business.IServices;
using Contract.DTOs;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ClsAppointmentService : ControllerBase, IAppointmentService
    {
        //private readonly IBaseRepository<Appointment> _repository;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Appointment> _appointments;

        public AppointmentService(IMapper mapper, IDbClient dbClient)
        {
            //_repository = repository;
            _mapper = mapper;
            _appointments = dbClient.GetAppointmentsCollection();
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAppointmentsAsync(string search)
        //{
        //    var appointments = await _repository
        //                        .Entities
        //                        .Include(a => a.Therapist)
        //                        .Where(a => a.Description.ToLower().Contains(search.ToLower()))
        //                        .ToListAsync();
        //    var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDetailsDto>>(appointments);
        //    return Ok(appointmentDtos);
        //}

        public async Task<IEnumerable<AppointmentDetailsDto>> GetAppointments()
        {
            var appointmentModel = await _appointments.Find(appointment => true).ToListAsync();
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDetailsDto>>(appointmentModel);
            return appointmentDtos;
        }

        public async Task<AppointmentDetailsDto> AddAppointment(AppointmentCreateDto model)
        {           
            var appointmentModel = _mapper.Map<Appointment>(model);
            await _appointments.InsertOneAsync(appointmentModel);
            var appointmentDtos = _mapper.Map<AppointmentDetailsDto>(appointmentModel);
            return appointmentDtos;
        }
        public async Task<IActionResult> CreateAppointmentAsync(AppointmentCreateDto appointmentCreateDto)
        {
            var appointment = _mapper.Map<Appointment>(appointmentCreateDto);
            var newAppointment = await _repository.Create(appointment);
            var appointmentDto = _mapper.Map<AppointmentDetailsDto>(newAppointment);
            return Ok(appointmentDto);
        }
    }
}

﻿using AutoMapper;
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
    public class AppointmentService : ControllerBase, IAppointmentService
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

        public async Task<AppointmentDetailsDto> GetAppointmentById(string id)
        {
            var appointmentModel = await _appointments.Find(x =>x.Id == id).FirstOrDefaultAsync();
            var appointmentDto = _mapper.Map<AppointmentDetailsDto>(appointmentModel);
            return appointmentDto;
        }

        public async Task<AppointmentDetailsDto> RescheduleAppointment(AppointmentCreateDto model,string id)
        {                                        
            var appointmentModel = _mapper.Map<Appointment>(model);
            appointmentModel.Id = id;
            await _appointments.ReplaceOneAsync(x => x.Id == id, appointmentModel);
            var appointmentDto = _mapper.Map<AppointmentDetailsDto>(appointmentModel);
            return appointmentDto;
        }

        public async Task CancelAppointment(string id)
        {
            await _appointments.DeleteOneAsync(x => x.Id == id);
        }

    }
}
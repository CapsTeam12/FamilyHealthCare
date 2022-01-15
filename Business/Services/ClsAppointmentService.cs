using AutoMapper;
using Business.IServices;
using Contract.DTOs;
using Contract.DTOs.AppoimentService;
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
        private readonly IBaseRepository<Appointment> _repository;
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Appointment> _appointments;
        private readonly ApplicationDbContext _db;

        public ClsAppointmentService(IMapper mapper, IDbClient dbClient, ApplicationDbContext db)
        {
            //_repository = repository;
            _mapper = mapper;
            _appointments = dbClient.GetAppointmentsCollection();
            _db = db;
        }

        [HttpGet]
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

        public async Task<IEnumerable<AppointmentDetailsDto>> GetAppointments(string userId) //Get Appointment of user
        {            
            List<Appointment> appointmentModel = await _appointments.Find(x => x.AccountId == userId).ToListAsync();
            var roleOfUser = await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId); // get role of User           
            if (roleOfUser != null)
            {
                var roleName = await _db.Roles.FirstOrDefaultAsync(x => x.Id == roleOfUser.RoleId); // get roleName of User
                if(roleName.Name.Equals("Doctor")) appointmentModel = await _appointments.Find(x => x.TherapistId == userId).ToListAsync(); // Get Appointment of doctor
            }            
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDetailsDto>>(appointmentModel);
            return appointmentDtos;
        }

        //public async Task<IEnumerable<AppointmentDetailsDto>> GetAppointmentsDoctor(string therapistId) // Get Appointment of doctor
        //{
        //    var appointmentModel = await _appointments.Find(x => x.TherapistId == therapistId).ToListAsync();
        //    var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDetailsDto>>(appointmentModel);
        //    return appointmentDtos;
        //}

        public async Task<AppointmentDetailsDto> GetAppointmentById(string id) // View Appointment
        {
            var appointmentModel = await _appointments.Find(x => x.Id == id).FirstOrDefaultAsync();
            var appointmentDto = _mapper.Map<AppointmentDetailsDto>(appointmentModel);
            return appointmentDto;
        }

        public async Task<AppointmentDetailsDto> BookingAppointment(AppointmentCreateDto model, string userId) // Booking Appointment
        {
            var appointmentModel = _mapper.Map<Appointment>(model);
            var therapist = await _db.Doctors.FirstOrDefaultAsync(x => x.AccountId == model.TherapistId); // Find therapist
            appointmentModel.AccountId = userId;
            appointmentModel.Therapist = therapist;
            await _appointments.InsertOneAsync(appointmentModel); // Insert appointment data
            var patient = await _db.Patients.FirstOrDefaultAsync(x => x.AccountId == userId); // Find patient
            List<Schedule> scheduleOfAppointment = new List<Schedule>()
            {
                new Schedule // Schedule of Patient
                {
                  AccountId = appointmentModel.AccountId,
                  AppointmentId = appointmentModel.Id,
                  Eventname = "Meeting with Dr." + appointmentModel.Therapist.FullName,
                  StartTime = appointmentModel.StartTime,
                  EndTime = appointmentModel.EndTime
                },
                new Schedule // Schedule of Doctor
                {
                    AccountId = appointmentModel.TherapistId,
                    AppointmentId = appointmentModel.Id,
                    Eventname = "Meeting with " + patient.FullName,
                    StartTime = appointmentModel.StartTime,
                    EndTime = appointmentModel.EndTime
                }
            };
            await _db.Schedules.AddRangeAsync(scheduleOfAppointment);
            await _db.SaveChangesAsync();
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

        public async Task<AppointmentDetailsDto> RescheduleAppointment(AppointmentRescheduleDto model, string id) // Reschedule Appointment
        {
            var appointmentModel = _mapper.Map<Appointment>(model);
            var therapist = await _db.Doctors.FirstOrDefaultAsync(x => x.AccountId == model.TherapistId); // Find therapist
            appointmentModel.Therapist = therapist;
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

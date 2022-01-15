using AutoMapper;
using Business.IServices;
using Contract.DTOs.ScheduleDoctorService;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ScheduleDoctorService : IScheduleDoctorService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ScheduleDoctorService(ApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
   

        public async Task<IEnumerable<Shift>> GetShiftsAsync()
        {
            var listShifts = await _db.Shifts.ToListAsync();
            return listShifts;
        }

        public async Task<IEnumerable<ScheduleDoctorDto>> GetSchedulesAsync(string userId,DateTime date) // View schedule timeslot of doctor when booking 
        {
            var ListScheduleOfDoctor = await _db.ScheduleDoctors.Where(x => x.UserId == userId && x.Date == date).ToListAsync();
            var ScheduleDoctorDtos = _mapper.Map<IEnumerable<ScheduleDoctorDto>>(ListScheduleOfDoctor);
            return ScheduleDoctorDtos;
        }

        public async Task<IEnumerable<ScheduleDoctorDto>> CreateScheduleAsync(ScheduleDoctorCreateDto schedule)
        {
            var ScheduleDoctorModel = _mapper.Map<ScheduleDoctor>(schedule);
            for (int i = 0; i < schedule.ShiftsId.Length; i++)
            {
                var newScheduleDoctor = new ScheduleDoctor()
                {
                    UserId = ScheduleDoctorModel.UserId,
                    Date = Convert.ToDateTime(ScheduleDoctorModel.Date.ToString("yyyy-MM-dd")),
                    IsBooking = ScheduleDoctorModel.IsBooking,
                    ShiftId = schedule.ShiftsId[i]
                };
                await _db.ScheduleDoctors.AddAsync(newScheduleDoctor);
                await _db.SaveChangesAsync();
            }
            var listScheduleOfDoctor = await _db.ScheduleDoctors.Where(x => x.UserId == schedule.UserId && x.Date == Convert.ToDateTime(schedule.Date.ToString("yyyy-MM-dd"))).ToListAsync();
            var ScheduleDoctorDto = _mapper.Map<IEnumerable<ScheduleDoctorDto>>(listScheduleOfDoctor);
            return ScheduleDoctorDto;
        }

    }
}

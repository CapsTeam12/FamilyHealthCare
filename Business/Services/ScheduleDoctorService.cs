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

        public ScheduleDoctorService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        public async Task<IEnumerable<Shift>> GetShiftsAsync()
        {
            var listShifts = await _db.Shifts.ToListAsync();
            return listShifts;
        }

        public async Task<IEnumerable<ScheduleDoctorDto>> GetSchedulesAsync(string userId, DateTime date) // View schedule timeslot of doctor when booking 
        {
            var ListScheduleOfDoctor = await _db.ScheduleDoctors.Where(x => x.AccountId == userId && x.Date == date).ToListAsync();
            var ScheduleDoctorDtos = _mapper.Map<IEnumerable<ScheduleDoctorDto>>(ListScheduleOfDoctor);
            return ScheduleDoctorDtos;
        }

        public async Task<IEnumerable<ScheduleDoctorDto>> CreateScheduleAsync(ScheduleDoctorCreateDto schedule)
        {
            var ScheduleDoctorModel = _mapper.Map<ScheduleDoctor>(schedule);            
            for (int i = 0; i < schedule.ShiftsId.Length; i++)
            {
                var getShiftsOfDoctor = await _db.ScheduleDoctors.FirstOrDefaultAsync(x => x.AccountId == schedule.AccountId && x.ShiftId == schedule.ShiftsId[i] 
                && x.Date == Convert.ToDateTime(schedule.Date.ToString("yyyy-MM-dd"))); // Get the doctor's current day shifts
                if (getShiftsOfDoctor == null) // Check if not exists shift in schedules of doctor then create
                {
                    var newScheduleDoctor = new ScheduleDoctor()
                    {
                        AccountId = ScheduleDoctorModel.AccountId,
                        Date = Convert.ToDateTime(ScheduleDoctorModel.Date.ToString("yyyy-MM-dd")),
                        IsBooking = ScheduleDoctorModel.IsBooking,
                        ShiftId = schedule.ShiftsId[i]
                    };
                    await _db.ScheduleDoctors.AddAsync(newScheduleDoctor);
                }else if(getShiftsOfDoctor != null 
                    && getShiftsOfDoctor.ShiftId == schedule.ShiftsId[i] 
                    && getShiftsOfDoctor.IsBooking == false) // Check if exists shift in schedules of doctor and not scheduled yet then cancel
                {
                    _db.ScheduleDoctors.Remove(getShiftsOfDoctor);
                }              
            }
            await _db.SaveChangesAsync();
            var listScheduleOfDoctor = await _db.ScheduleDoctors.Where(x => x.AccountId == schedule.AccountId && x.Date == Convert.ToDateTime(schedule.Date.ToString("yyyy-MM-dd"))).ToListAsync();
            var ScheduleDoctorDto = _mapper.Map<IEnumerable<ScheduleDoctorDto>>(listScheduleOfDoctor);
            return ScheduleDoctorDto;
        }

        public void DeleteExpirationSchedules() // Delete expiration schedules 
        {
            var CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")); // get current date
            var ExpirationSchedules = _db.ScheduleDoctors.Where(x => DateTime.Compare(x.Date, CurrentDate) < 0).ToList(); // get list expiration schedule of doctors
            _db.ScheduleDoctors.RemoveRange(ExpirationSchedules);
            //_db.SaveChangesAsync();
        }

    }
}

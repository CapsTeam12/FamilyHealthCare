using AutoMapper;
using Business.IServices;
using Contract.DTOs.ScheduleService;
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
    public class ScheduleService : IScheduleService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ScheduleService(ApplicationDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ScheduleDto> CreateScheduleAsync(ScheduleCreateDto schedule)  // Create Schedule
        {
            var ScheduleModel = _mapper.Map<Schedule>(schedule);
            await _db.Schedules.AddAsync(ScheduleModel);
            await _db.SaveChangesAsync();
            var ScheduleDto = _mapper.Map<ScheduleDto>(ScheduleModel);
            return ScheduleDto;
        }

        public async Task<IEnumerable<ScheduleDto>> GetScheduleAndUpdateAsync(string userId,string doctorAccountId,ScheduleCreateDto createDto)
        {
            // Find
            var scheduleOfUser = await _db.Schedules.FirstOrDefaultAsync(x => x.AccountId == userId && x.AppointmentId == createDto.AppointmentId);
            var scheduleOfDoctor = await _db.Schedules.FirstOrDefaultAsync(x => x.AccountId == doctorAccountId && x.AppointmentId == createDto.AppointmentId);
            // Update
            scheduleOfUser.Join_Url = createDto.Join_Url;
            scheduleOfDoctor.Start_Url = createDto.Start_Url;
            _db.Schedules.Update(scheduleOfUser);
            _db.Schedules.Update(scheduleOfDoctor);
           
            await _db.SaveChangesAsync();

            // Get schedules 
            var schedulesOfAppointment = await _db.Schedules.Where(x => x.AppointmentId == createDto.AppointmentId).ToListAsync();
            var scheduleDto = _mapper.Map<IEnumerable<ScheduleDto>>(schedulesOfAppointment);
            return scheduleDto;
        }

        public async Task<IEnumerable<ScheduleDto>> GetSchedulesAsync(string userId) // View List Schedule On Calendar of User
        {
            var ListScheduleOfUser = await _db.Schedules.Where(x => x.AccountId == userId).ToListAsync();
            var ScheduleDtos = _mapper.Map<List<ScheduleDto>>(ListScheduleOfUser);
            return ScheduleDtos;
        }

    }
}

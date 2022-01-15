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

        public async Task<IEnumerable<ScheduleDto>> GetSchedulesAsync(string userId) // View List Schedule On Calendar of User
        {
            var ListScheduleOfUser = await _db.Schedules.Where(x => x.AccountId == userId).ToListAsync();
            var ScheduleDtos = _mapper.Map<List<ScheduleDto>>(ListScheduleOfUser);
            return ScheduleDtos;
        }

    }
}

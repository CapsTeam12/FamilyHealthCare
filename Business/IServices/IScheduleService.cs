using Contract.DTOs.ScheduleService;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleDto>> GetSchedulesAsync(string userId);
        Task<ScheduleDto> CreateScheduleAsync(ScheduleCreateDto schedule);

    }
}

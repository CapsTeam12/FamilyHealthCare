using Contract.DTOs.ScheduleDoctorService;
using Contract.DTOs.ScheduleService;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IScheduleDoctorService
    {
        Task<IEnumerable<ScheduleDoctorDto>> GetSchedulesAsync(string userId,DateTime date);
        Task<IEnumerable<ScheduleDoctorDto>> CreateScheduleAsync(ScheduleDoctorCreateDto schedule);
        Task<IEnumerable<Shift>> GetShiftsAsync();
        void DeleteExpirationSchedules();

    }
}

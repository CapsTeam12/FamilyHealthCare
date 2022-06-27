using AutoMapper;
using Business.IServices;
using Contract.DTOs.MailService;
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
        private readonly ISendMailService _mailService;

        public ScheduleService(ApplicationDbContext db, IMapper mapper, ISendMailService mailService)
        {
            _db = db;
            _mapper = mapper;
            _mailService = mailService;
        }

        public async Task<ScheduleDto> CreateScheduleAsync(ScheduleCreateDto schedule)  // Create Schedule
        {
            var ScheduleModel = _mapper.Map<Schedule>(schedule);
            await _db.Schedules.AddAsync(ScheduleModel);
            await _db.SaveChangesAsync();
            var ScheduleDto = _mapper.Map<ScheduleDto>(ScheduleModel);
            return ScheduleDto;
        }

        public async Task<IEnumerable<ScheduleDto>> GetScheduleAndUpdateAsync(string userId, string doctorAccountId, ScheduleCreateDto createDto)
        {
            // Find
            var scheduleOfUser = await _db.Schedules.FirstOrDefaultAsync(x => x.AccountId == userId && x.AppointmentId == createDto.AppointmentId);
            var scheduleOfDoctor = await _db.Schedules.FirstOrDefaultAsync(x => x.AccountId == doctorAccountId && x.AppointmentId == createDto.AppointmentId);
            // Update
            scheduleOfUser.MeetingId = createDto.MeetingId;
            scheduleOfUser.Join_Url = createDto.Join_Url;

            scheduleOfDoctor.MeetingId = createDto.MeetingId;
            scheduleOfDoctor.Start_Url = createDto.Start_Url;
            _db.Schedules.Update(scheduleOfUser);
            _db.Schedules.Update(scheduleOfDoctor);

            await _db.SaveChangesAsync();
            // Get infor of user and doctor
            var user = await _db.Patients.Include(u => u.User).FirstOrDefaultAsync(x => x.AccountId == userId);
            var doctor = await _db.Doctors.FirstOrDefaultAsync(x => x.AccountId == doctorAccountId);
            // Send Appointment infor by mail to both
            List<MailContent> mailContent = new List<MailContent>()
            {
                new MailContent()
                {
                    To = user.User.Email,
                    Subject = $"{user.FullName} booking appointment with Dr. {doctor.FullName} at {createDto.StartTime.ToString("HH:mm dd/MM/yyyy")}",
                    Body = $"<h3>Dear {user.FullName},</h3>" +

                        $"<p>You have just booked an appointment at {createDto.StartTime.ToString("HH:mm dd/MM/yyyy")} with Dr. {doctor.FullName} and the appointment will take place in about 30 minutes. Join meeting <a target='_blank' href='{createDto.Join_Url}'>here</a></p>" +

                        @"<p>If you have any questions or expectations then please contact us. We hope your meeting goes well. Thank you for using our service.</p>

                        <h3>Best regards,</h3>
                        <i>FHC Team</i>
                         <p>
                        <img src='https://lh3.googleusercontent.com/pw/AM-JKLVbarNakIE9FJgDXlR0RVbR57BcHN_5PllXqzVwgsk2oDTEj7hwJ-b8RzOsn2g8wsmWGFUfaAh6-WbF-dgLWDBrZEZFZKz68m4NqGzXX-lQduWo6LB5xZC31ScGgfQMsl5ICWbjL93xMJLtHjKxMUI=w160-h41-no?authuser=0'
                        width='100px' style='float: left; margin-left: 5px; margin-right: 20px; border: 2px solid black;' />
                        <b style='float: left;'>FHC Team</b>&nbsp;|&nbsp;<span>Email: <b>fhc.health12@gmail.com</b></span><br>
                        <span>Hotline: <b>09990909</b></span>&nbsp;|&nbsp;<span>Website: <a
                        href='http://fhc.eastasia.cloudapp.azure.com/'>http://fhc.eastasia.cloudapp.azure.com/</a></span> <br>
                        </p>"
                },
                new MailContent()
                {
                    To = doctor.Email,
                    Subject = $"{user.FullName} booking appointment with Dr. {doctor.FullName} at {createDto.StartTime.ToString("HH:mm dd/MM/yyyy")}",
                    Body = $"<h3>Dear {doctor.FullName},</h3>" +

                        $"<p>You have just booked an appointment at {createDto.StartTime.ToString("HH:mm dd/MM/yyyy")} with patient {user.FullName} and the appointment will take place in about 30 minutes. Start meeting <a target='_blank' href='{createDto.Start_Url}'>here</a></p>" +

                        @"<p>If you have any questions or expectations then please contact us. We hope your meeting goes well. Thank you for using our service.</p>

                        <h3>Best regards,</h3>
                        <i>FHC Team</i>
                         <p>
                        <img src='https://lh3.googleusercontent.com/pw/AM-JKLVbarNakIE9FJgDXlR0RVbR57BcHN_5PllXqzVwgsk2oDTEj7hwJ-b8RzOsn2g8wsmWGFUfaAh6-WbF-dgLWDBrZEZFZKz68m4NqGzXX-lQduWo6LB5xZC31ScGgfQMsl5ICWbjL93xMJLtHjKxMUI=w160-h41-no?authuser=0'
                        width='100px' style='float: left; margin-left: 5px; margin-right: 20px; border: 2px solid black;' />
                        <b style='float: left;'>FHC Team</b>&nbsp;|&nbsp;<span>Email: <b>fhc.health12@gmail.com</b></span><br>
                        <span>Hotline: <b>09990909</b></span>&nbsp;|&nbsp;<span>Website: <a
                        href='http://fhc.eastasia.cloudapp.azure.com/'>http://fhc.eastasia.cloudapp.azure.com/</a></span> <br>
                        </p>"
                },
            };
            await _mailService.SendListMail(mailContent);
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

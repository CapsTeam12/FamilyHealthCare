using AutoMapper;
using Business.IServices;
using Contract.Constants;
using Contract.DTOs;
using Contract.DTOs.AppoimentService;
using Contract.DTOs.MailService;
using Contract.DTOs.NotificationServiceDtos;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
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
        private readonly IZoomService _zoomService;
        private readonly ISendMailService _mailService;

        public ClsAppointmentService(IMapper mapper, IDbClient dbClient, ApplicationDbContext db, IZoomService zoomService, ISendMailService mailService)
        {
            //_repository = repository;
            _mapper = mapper;
            _appointments = dbClient.GetAppointmentsCollection();
            _db = db;
            _zoomService = zoomService;
            _mailService = mailService;
        }

        public async Task<IEnumerable<AppointmentDetailsDto>> GetTotalAppointments()
        {
            var appointments = await _appointments.Find(x => true).ToListAsync();
            var appoimentsDtos = _mapper.Map<IEnumerable<AppointmentDetailsDto>>(appointments);
            return appoimentsDtos;
        }

        public int GetTotalAppointmentsByPatient(string id)
        {
            var appointments = _appointments.Find(x => x.AccountId == id).ToList();
            return appointments.Count;
        }

        public int GetTotalAppointmentsByDoctor(string id)
        {
            var appointments = _appointments.Find(x => x.Therapist.AccountId == id).ToList();
            return appointments.Count;
        }
        private bool CheckIfExistAppointment(string userId, DateTime date, DateTime startTime)
        {
            var listAppointmentInDb = _db.Schedules
                                    .Where(a => a.AccountId == userId && a.StartTime.Date == date)
                                    .ToList(); // Lay danh sach cac cuoc hen trong ngay
            if (listAppointmentInDb.Count == 0) return true;
            foreach (var ap in listAppointmentInDb)
            {
                var apHourStartTime = ap.StartTime; // Gio bat dau cua appointment 
                var hourDistance = (startTime - apHourStartTime).TotalHours; // Khoang cach giua 2 cuoc hen trong ngay
                if (hourDistance == 0)
                {
                    return false;
                }
            }
            return true;
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

        //1: Coming, 2: In Progress, 3: Completed 4: Canceled
        public async Task<IEnumerable<AppointmentDetailsDto>> GetAppointments(string userId) //Get Appointment of user
        {
            List<Appointment> appointmentModel = await _appointments.Find(x => x.AccountId == userId).SortBy(s => s.Status).ToListAsync();

            var roleOfUser = await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId); // get role of User           
            if (roleOfUser != null)
            {
                var roleName = await _db.Roles.FirstOrDefaultAsync(x => x.Id == roleOfUser.RoleId); // get roleName of User
                if (roleName.Name.Equals("Doctor"))
                {
                    appointmentModel = await _appointments.Find(x => x.Therapist.AccountId == userId).SortBy(s => s.Status).ToListAsync(); // Get Appointment of doctor

                }
            }
            foreach (var appointment in appointmentModel)
            {
                if (DateTime.Compare(DateTime.Now, appointment.EndTime.ToLocalTime()) > 0 && appointment.Status != 4) // Cuộc hẹn đã diễn ra và kh bị hủy
                {
                    appointment.Status = 3; // status Completed

                }
                else if ((appointment.StartTime.ToLocalTime() <= DateTime.Now && DateTime.Now <= appointment.EndTime.ToLocalTime()) && appointment.Status != 4) // Cuộc hẹn đang diễn ra và kh bị hủy
                {
                    appointment.Status = 2; // status In Progress
                }
                await _appointments.ReplaceOneAsync(x => x.Id == appointment.Id, appointment);
            }
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDetailsDto>>(appointmentModel);
            return appointmentDtos;
        }

        //1: Coming, 2: In Progress, 3: Completed 4: Canceled
        public async Task<AppointmentDetailsDto> GetAppointmentById(string id) // View Appointment
        {
            var appointmentModel = await _appointments.Find(x => x.Id == id).FirstOrDefaultAsync();
            var appointmentDto = _mapper.Map<AppointmentDetailsDto>(appointmentModel);
            return appointmentDto;
        }

        //1: Coming, 2: In Progress, 3: Completed 4: Canceled
        public async Task<AppointmentDetailsDto> BookingAppointment(AppointmentCreateDto model, string userId) // Booking Appointment
        {
            var therapist = await _db.Doctors.FirstOrDefaultAsync(x => x.Id == model.TherapistId); // Find therapist
            var patient = await _db.Patients.FirstOrDefaultAsync(x => x.AccountId == userId); // Find patient
            var appointmentModel = _mapper.Map<Appointment>(model);
            appointmentModel.AccountId = userId;
            appointmentModel.Therapist = therapist;
            appointmentModel.Patient = patient;
            appointmentModel.StartTime = Convert.ToDateTime(model.Date.ToString("yyyy-MM-dd") + " " + appointmentModel.StartTime.ToString("HH:mm:ss"));
            appointmentModel.EndTime = Convert.ToDateTime(model.Date.ToString("yyyy-MM-dd") + " " + appointmentModel.EndTime.ToString("HH:mm:ss"));
            appointmentModel.Status = 1; // status Coming
            string timeSlot = model.StartTime.ToString("HH:mm:ss") + "-" + model.EndTime.ToString("HH:mm:ss");
            var scheduleOfDoctor = await _db.ScheduleDoctors.FirstOrDefaultAsync(x => x.Date == Convert.ToDateTime(model.Date.ToString("yyyy-MM-dd"))
            && x.Shift.TimeSlot == timeSlot && x.AccountId == therapist.AccountId); // Find schedule of doctor when patient book 
            var checkIfExist = CheckIfExistAppointment(userId, model.Date, model.StartTime);
            if (scheduleOfDoctor != null && checkIfExist == true) // Check if exist schedule of Doctor in view booking
            {
                await _appointments.InsertOneAsync(appointmentModel); // Insert appointment data

                scheduleOfDoctor.IsBooking = true;
                _db.ScheduleDoctors.Update(scheduleOfDoctor);

                List<Schedule> scheduleOfAppointment = new List<Schedule>() // Create schedule of patient and doctor
                {
                new Schedule // Schedule of Patient
                    {
                  AccountId = appointmentModel.AccountId,
                  AppointmentId = appointmentModel.Id,
                  Eventname = "Meeting with Dr." + appointmentModel.Therapist.FullName,
                  StartTime = Convert.ToDateTime(model.Date.ToString("yyyy-MM-dd") + " " + appointmentModel.StartTime.ToString("HH:mm:ss")),
                  EndTime = Convert.ToDateTime(model.Date.ToString("yyyy-MM-dd") + " " + appointmentModel.EndTime.ToString("HH:mm:ss"))
                    },
                new Schedule // Schedule of Doctor
                    {
                    AccountId = appointmentModel.Therapist.AccountId,
                    AppointmentId = appointmentModel.Id,
                    Eventname = "Meeting with " + patient.FullName,
                    StartTime = Convert.ToDateTime(model.Date.ToString("yyyy-MM-dd") + " " + appointmentModel.StartTime.ToString("HH:mm:ss")),
                    EndTime = Convert.ToDateTime(model.Date.ToString("yyyy-MM-dd") + " " + appointmentModel.EndTime.ToString("HH:mm:ss"))
                    }
                };
                await _db.Schedules.AddRangeAsync(scheduleOfAppointment); // Insert schedule 
                await _db.SaveChangesAsync();
                var appointmentDtos = _mapper.Map<AppointmentDetailsDto>(appointmentModel);


                var notification = new NotificationCreateDto();

                notification.UserID = therapist.AccountId;
                notification.Content = string.Format(
                                        NotificationContentTemplate.NewAppointment,
                                        patient.FullName,
                                        model.StartTime.ToString("HH:mm dd/MM/yyyy"));
                notification.AvatarSender = patient.Avatar;

                Task.Run(() => new NotificationHelper().CallApiCreateNotification(notification));

                return appointmentDtos;
            }
            return null;
        }

        public async Task<IActionResult> CreateAppointmentAsync(AppointmentCreateDto appointmentCreateDto)
        {
            var appointment = _mapper.Map<Appointment>(appointmentCreateDto);
            var newAppointment = await _repository.Create(appointment);
            var appointmentDto = _mapper.Map<AppointmentDetailsDto>(newAppointment);
            return Ok(appointmentDto);
        }

        //1: Coming, 2: In Progress, 3: Completed 4: Canceled
        public async Task<AppointmentDetailsDto> RescheduleAppointment(AppointmentRescheduleDto model, string id) // Reschedule Appointment
        {
            var oldAppointment = await GetAppointmentById(id); // get old Appointment
            var timeSlotOldAppointment = oldAppointment.StartTime.ToLocalTime().ToString("HH:mm:ss") + "-" + oldAppointment.EndTime.ToLocalTime().ToString("HH:mm:ss"); // get timeslot of old appointment
            // Find the doctor's schedule appointment that the user makes rescheduling
            var OldscheduleOfDoctor = await _db.ScheduleDoctors.FirstOrDefaultAsync(x => x.AccountId == oldAppointment.Therapist.AccountId
            && x.Shift.TimeSlot == timeSlotOldAppointment && x.IsBooking == true && x.Date == oldAppointment.StartTime.Date);
            // Reshedule
            var appointmentModel = _mapper.Map<Appointment>(model);
            var therapist = await _db.Doctors.FirstOrDefaultAsync(x => x.Id == model.TherapistId); // Find therapist
            var patient = await _db.Patients.Include(u => u.User).FirstOrDefaultAsync(x => x.Id == model.PatientId); // Find patient
            appointmentModel.Patient = patient;
            appointmentModel.Therapist = therapist;
            appointmentModel.Id = id;
            var schedulesOfAppointment = await _db.Schedules.Where(x => x.AppointmentId == appointmentModel.Id).ToListAsync(); // Find scheduled appointments to reschedule
            DateTime startTime = oldAppointment.StartTime.ToLocalTime(); // Thời gian bắt đầu của cuộc hẹn sẽ thay đổi
            DateTime currentTime = DateTime.Now; // Thời gian hiện tại 
            var HourDistance = (startTime - currentTime).TotalHours; // Khoảng cách giờ giữa thời gian bắt đầu cuộc hẹn sẽ thay đổi và thời gian hiện tại 
            var checkIfExist = CheckIfExistAppointment(model.AccountId, oldAppointment.StartTime.Date, model.StartTime);
            if (schedulesOfAppointment != null // Check if the scheduled appointment is exist
                && appointmentModel.Status == 1 // and the appointment status is coming
                && HourDistance >= 2 && checkIfExist == true) // and the time to reschedule it must be 2 hours before
            {
                await _appointments.ReplaceOneAsync(x => x.Id == id, appointmentModel);
                foreach (var schedules in schedulesOfAppointment) // Get schedules appointment to update 
                {
                    schedules.StartTime = model.StartTime;
                    schedules.EndTime = model.EndTime;
                    _db.Schedules.Update(schedules);
                }
                OldscheduleOfDoctor.IsBooking = false; // set isbooking false after appointment rescheduled
                // Find schedule of doctor after change 
                var timeSlotNewAppointment = model.StartTime.ToString("HH:mm:ss") + "-" + model.EndTime.ToString("HH:mm:ss"); // get new Timeslot of new appointment StartTime and EndTime 
                var NewscheduleOfDoctor = await _db.ScheduleDoctors.FirstOrDefaultAsync(x => x.AccountId == appointmentModel.Therapist.AccountId
                && x.Date == model.StartTime.Date && x.Shift.TimeSlot == timeSlotNewAppointment);
                NewscheduleOfDoctor.IsBooking = true; // set isbooking true with new schedule appointment
                await _db.SaveChangesAsync();
                var meeting = new Meeting
                {
                    Id = schedulesOfAppointment.Select(s => s.MeetingId).First(),
                    StartTime = model.StartTime,
                    Duration = model.EndTime.Minute - model.StartTime.Minute
                };
                await _zoomService.UpdateMeeting(meeting.Id, meeting);
                var appointmentDto = _mapper.Map<AppointmentDetailsDto>(appointmentModel);
                var zoomMeeting = await _zoomService.Meeting(meeting.Id);
                List<MailContent> mailContent = new List<MailContent>()
            {
                new MailContent()
                {
                    To = patient.User.Email,
                    Subject = $"Scheduled appointment at {model.StartTime.ToString("HH:mm dd/MM/yyyy")}",
                    Body = $"<h3>Dear {patient.FullName},</h3>" +
                        $"<p>Scheduled an appointment at {model.StartTime.ToString("HH:mm dd/MM/yyyy")} with Dr. {therapist.FullName} and the appointment will take place in about 30 minutes. Join meeting <a target='_blank' href='{zoomMeeting.Join_Url}'>here</a></p>" +
                        @"
                        <p>If you have any questions or expectations then please contact us. Thank you for using our service.</p>
                        <h3>Best regards,</h3>
                        <i>FHC Team</i>
                         <p>
                        <img src='https://lh3.googleusercontent.com/pw/AM-JKLVbarNakIE9FJgDXlR0RVbR57BcHN_5PllXqzVwgsk2oDTEj7hwJ-b8RzOsn2g8wsmWGFUfaAh6-WbF-dgLWDBrZEZFZKz68m4NqGzXX-lQduWo6LB5xZC31ScGgfQMsl5ICWbjL93xMJLtHjKxMUI=w160-h41-no?authuser=0'
                        width='100px' style='float: left; margin-left: 5px; margin-right: 20px; border: 2px solid black;' />
                        <b style='float: left;'>FHC Team</b>&nbsp;|&nbsp;<span>Email: <b>fhc.health12@gmail.com</b></span><br>
                        <span>Hotline: <b>09990909</b></span>&nbsp;|&nbsp;<span>Website: <a
                        href='https://abc.com'>https://abc.com</a></span> <br>
                        </p>"
                },
                new MailContent()
                {
                    To = therapist.Email,
                    Subject = $"Scheduled appointment at {model.StartTime.ToString("HH:mm dd/MM/yyyy")}",
                    Body = $"<h3>Dear {therapist.FullName},</h3>" +
                        $"<p>Scheduled an appointment at {model.StartTime.ToString("HH:mm dd/MM/yyyy")} with patient {patient.FullName} and the appointment will take place in about 30 minutes. Start meeting <a target='_blank' href='{zoomMeeting.Start_Url}'>here</a></p>" +
                        @"
                        <p> If you have any questions or expectations then please contact us. Thank you for using our service.</p>
                        <h3>Best regards,</h3>
                        <i>FHC Team</i>
                         <p>
                        <img src='https://lh3.googleusercontent.com/pw/AM-JKLVbarNakIE9FJgDXlR0RVbR57BcHN_5PllXqzVwgsk2oDTEj7hwJ-b8RzOsn2g8wsmWGFUfaAh6-WbF-dgLWDBrZEZFZKz68m4NqGzXX-lQduWo6LB5xZC31ScGgfQMsl5ICWbjL93xMJLtHjKxMUI=w160-h41-no?authuser=0'
                        width='100px' style='float: left; margin-left: 5px; margin-right: 20px; border: 2px solid black;' />
                        <b style='float: left;'>FHC Team</b>&nbsp;|&nbsp;<span>Email: <b>fhc.health12@gmail.com</b></span><br>
                        <span>Hotline: <b>09990909</b></span>&nbsp;|&nbsp;<span>Website: <a
                        href='https://abc.com'>https://abc.com</a></span> <br>
                        </p>"
                },
            };
                await _mailService.SendListMail(mailContent);
                return appointmentDto;
            }
            return null;
        }

        //1: Coming, 2: In Progress, 3: Completed 4: Canceled
        public async Task<AppointmentDetailsDto> CancelAppointment(string id, string userId) // Cancel appointment
        {
            var appointment = await _appointments.Find(x => x.Id == id).FirstOrDefaultAsync(); // Find appointment to cancel
            DateTime startTime = appointment.StartTime.ToLocalTime(); // Thời gian bắt đầu của cuộc hẹn
            DateTime currentTime = DateTime.Now; // Thời gian hiện tại 
            var HourDistance = (startTime - currentTime).TotalHours; // Khoảng cách giờ giữa thời gian bắt đầu cuộc hẹn và thời gian hiện tại 
            if (appointment != null && appointment.Status == 1 && HourDistance >= 2)
            {
                // Change isBooking of schedule doctor
                string timeSlot = appointment.StartTime.ToLocalTime().ToString("HH:mm:ss") + "-" + appointment.EndTime.ToLocalTime().ToString("HH:mm:ss");
                var scheduleOfDoctor = await _db.ScheduleDoctors.FirstOrDefaultAsync(x => x.Date == appointment.StartTime.Date
                    && x.Shift.TimeSlot == timeSlot && x.AccountId == appointment.Therapist.AccountId); // Find schedule of doctor when patient book 
                scheduleOfDoctor.IsBooking = false;// Set isbooking false after canceled
                _db.ScheduleDoctors.Update(scheduleOfDoctor);
                // =====
                // Find scheduled appointments to cancel
                var schedulesOfAppointment = await _db.Schedules.Where(x => x.AppointmentId == appointment.Id).ToListAsync();
                _db.Schedules.RemoveRange(schedulesOfAppointment);
                // =====
                appointment.Status = 4; // Status Canceled
                await _db.SaveChangesAsync();
                // Replace status of appointment already canceled
                await _appointments.ReplaceOneAsync(x => x.Id == id, appointment);
                var meetingId = schedulesOfAppointment.Select(m => m.MeetingId).First();
                await _zoomService.DeleteMeeting(meetingId);


                var notification = new NotificationCreateDto();

                var currentPatient = await _db.Patients.Include(u => u.User).FirstOrDefaultAsync(x => x.AccountId == userId);
                if (currentPatient != null)
                {
                    notification.Content = string.Format(NotificationContentTemplate.CancelledAppointment);
                    notification.AvatarSender = currentPatient.Avatar;
                    var currentDoctor = await _db.Doctors.FirstOrDefaultAsync(x => x.Id == appointment.TherapistId);
                    if (currentDoctor != null)
                    {
                        notification.UserID = currentDoctor.AccountId;
                    }
                }
                else
                {
                    var currentDoctor = await _db.Doctors.FirstOrDefaultAsync(x => x.AccountId == userId);
                    if (currentDoctor != null)
                    {
                        notification.Content = string.Format(NotificationContentTemplate.CancelledAppointment);
                        notification.AvatarSender = currentDoctor.Avatar;
                        notification.UserID = appointment.AccountId;
                    }
                }

                Task.Run(() => new NotificationHelper().CallApiCreateNotification(notification));
                var doctor = await _db.Doctors.FirstOrDefaultAsync(d => d.Id == appointment.TherapistId);
                List<MailContent> mailContent = new List<MailContent>()
            {
                new MailContent()
                {
                    To = currentPatient.User.Email,
                    Subject = $"Canceled booking appointment at {appointment.StartTime.ToString("HH:mm dd/MM/yyyy")}",
                    Body = $"<h3>Dear {currentPatient.FullName},</h3>" +

                        $"<p>Canceled an appointment at {appointment.StartTime.ToString("HH:mm dd/MM/yyyy")}. If you have any questions or expectations then please contact us. Thank you for using our service.</p>" +

                        @"
                        <h3>Best regards,</h3>
                        <i>FHC Team</i>
                         <p>
                        <img src='https://lh3.googleusercontent.com/pw/AM-JKLVbarNakIE9FJgDXlR0RVbR57BcHN_5PllXqzVwgsk2oDTEj7hwJ-b8RzOsn2g8wsmWGFUfaAh6-WbF-dgLWDBrZEZFZKz68m4NqGzXX-lQduWo6LB5xZC31ScGgfQMsl5ICWbjL93xMJLtHjKxMUI=w160-h41-no?authuser=0'
                        width='100px' style='float: left; margin-left: 5px; margin-right: 20px; border: 2px solid black;' />
                        <b style='float: left;'>FHC Team</b>&nbsp;|&nbsp;<span>Email: <b>fhc.health12@gmail.com</b></span><br>
                        <span>Hotline: <b>09990909</b></span>&nbsp;|&nbsp;<span>Website: <a
                        href='https://abc.com'>https://abc.com</a></span> <br>
                        </p>"
                },
                new MailContent()
                {
                    To = doctor.Email,
                    Subject = $"Canceled booking appointment at {appointment.StartTime.ToString("HH:mm dd/MM/yyyy")}",
                    Body = $"<h3>Dear {doctor.FullName},</h3>" +

                        $"<p>Canceled an appointment at {appointment.StartTime.ToString("HH:mm dd/MM/yyyy")}. If you have any questions or expectations then please contact us. Thank you for using our service.</p>" +

                        @"
                        <h3>Best regards,</h3>
                        <i>FHC Team</i>
                         <p>
                        <img src='https://lh3.googleusercontent.com/pw/AM-JKLVbarNakIE9FJgDXlR0RVbR57BcHN_5PllXqzVwgsk2oDTEj7hwJ-b8RzOsn2g8wsmWGFUfaAh6-WbF-dgLWDBrZEZFZKz68m4NqGzXX-lQduWo6LB5xZC31ScGgfQMsl5ICWbjL93xMJLtHjKxMUI=w160-h41-no?authuser=0'
                        width='100px' style='float: left; margin-left: 5px; margin-right: 20px; border: 2px solid black;' />
                        <b style='float: left;'>FHC Team</b>&nbsp;|&nbsp;<span>Email: <b>fhc.health12@gmail.com</b></span><br>
                        <span>Hotline: <b>09990909</b></span>&nbsp;|&nbsp;<span>Website: <a
                        href='https://abc.com'>https://abc.com</a></span> <br>
                        </p>"
                },
            };
                await _mailService.SendListMail(mailContent);
            }
            var appointmentDto = _mapper.Map<AppointmentDetailsDto>(appointment);
            return appointmentDto;
        }
    }
}

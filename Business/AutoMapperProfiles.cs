using Contract.DTOs;
using Contract.DTOs.AppoimentService;
using Contract.DTOs.ScheduleDoctorService;
using Contract.DTOs.ScheduleService;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AutoMapperProfiles : AutoMapper.Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Appointment, AppointmentDetailsDto>()
                .ForMember(d => d.TherapistFullName, t => t.MapFrom(m => m.Therapist.FullName))
                .ReverseMap();
            CreateMap<Appointment, AppointmentCreateDto>().ReverseMap();
            CreateMap<Appointment, AppointmentRescheduleDto>().ReverseMap();

            CreateMap<Schedule, ScheduleDto>().ReverseMap();
            CreateMap<Schedule, ScheduleCreateDto>().ReverseMap();

            CreateMap<ScheduleDoctor, ScheduleDoctorDto>().ReverseMap();
            CreateMap<ScheduleDoctor, ScheduleDoctorCreateDto>().ReverseMap();
        }
    }
}

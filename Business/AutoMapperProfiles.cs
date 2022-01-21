using Contract.DTOs;
using Contract.DTOs.AppoimentService;
using Contract.DTOs.ScheduleDoctorService;
using Contract.DTOs.ScheduleService;
using Contract.DTOs.ManagementService;
using Contract.DTOs.SearchService;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract.DTOs.AuthService;

namespace Business
{
    public class AutoMapperProfiles : AutoMapper.Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Appointment, AppointmentDetailsDto>()
                .ReverseMap();
            CreateMap<Appointment, AppointmentCreateDto>()
                .ReverseMap();
            CreateMap<Medicine , SearchMedicineDto>()
                .ForMember(d => d.ClassificationName, t => t.MapFrom(m => m.MedicineClass.ClassificationName))
                .ReverseMap();
            CreateMap<Doctor, SearchDoctorDto>()
                .ReverseMap();
            CreateMap<Pharmacy, SearchPharmacyDto>()
               .ReverseMap();
            CreateMap<Doctor, DoctorDetailsDto>().ForMember(d =>d.Specialities, t => t.MapFrom(m => m.Specialized.SpecializedName))
                .ReverseMap();
            CreateMap<Pharmacy, PharmacyDetailsDto>()
                .ReverseMap();
            CreateMap<User, PatientUpdateDto>()
                .ForMember(x => x.AccountId, opt => opt.MapFrom(m => m.Id))
                .ReverseMap();
            CreateMap<MedicineClassification, CategoriesDetailsDto>()
                .ForMember(x => x.CateName, opt => opt.MapFrom(m => m.ClassificationName))
                .ReverseMap();
            CreateMap<Specialities, SpecialitiesDetailsDto>()
                .ReverseMap();
            CreateMap<User, ChangePasswordDto>()
                .ReverseMap();
            CreateMap<User, UpdatePatientProfileDto>()
               .ForMember(x => x.AccountId, opt => opt.MapFrom(m => m.Id))
               .ReverseMap();
            CreateMap<Patient, PatientDetailsDto>()
               .ForMember(x => x.AccountId, opt => opt.MapFrom(m => m.Id))
               .ReverseMap();

            CreateMap<Appointment, AppointmentRescheduleDto>().ReverseMap();

            CreateMap<Schedule, ScheduleDto>().ReverseMap();
            CreateMap<Schedule, ScheduleCreateDto>().ReverseMap();

            CreateMap<ScheduleDoctor, ScheduleDoctorDto>().ReverseMap();
            CreateMap<ScheduleDoctor, ScheduleDoctorCreateDto>().ReverseMap();

        }
    }
}

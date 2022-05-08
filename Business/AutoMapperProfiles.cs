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
using Contract.DTOs.PartnerService;
using Contract.DTOs.MedicineService;
using Contract.DTOs.MedicalRecordService;
using Contract.DTOs.PrescriptionService;

namespace Business
{
    public class AutoMapperProfiles : AutoMapper.Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Appointment, AppointmentDetailsDto>()
                .ForMember(dst => dst.Therapist, opt => opt.MapFrom(s => s.Therapist))
                .ForMember(dst => dst.Patient,opt => opt.MapFrom(s => s.Patient))
                .ReverseMap();
            CreateMap<Appointment, AppointmentCreateDto>()
                .ReverseMap();
            CreateMap<Medicine, SearchMedicineDto>()
                .ForMember(d => d.ClassificationName, t => t.MapFrom(m => m.MedicineClass.ClassificationName))
                .ReverseMap();
            CreateMap<Doctor, SearchDoctorDto>()
                .ReverseMap();
            CreateMap<Pharmacy, SearchPharmacyDto>()
               .ReverseMap();
            CreateMap<Doctor, DoctorDetailsDto>().ForMember(d => d.Specialities, t => t.MapFrom(m => m.Specialized.SpecializedName))
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


            CreateMap<Doctor, DoctorRegisterDto>().ReverseMap();
            CreateMap<Experience, ExperiencesDto>().ReverseMap();

            CreateMap<Pharmacy, PharmacyRegisterDto>().ReverseMap();
            CreateMap<Awards, AwardsDto>().ReverseMap();

            CreateMap<Doctor, DoctorRequestDetailsDto>().ForMember(x => x.Specialized, opt => opt.MapFrom(m => m.Specialized.SpecializedName)).ReverseMap();
            CreateMap<Pharmacy, PharmacyRequestDetailsDto>().ReverseMap();

            CreateMap<Medicine, MedicineDto>().ReverseMap();
            CreateMap<Pharmacy, PharmacyDto>().ReverseMap();
            CreateMap<MedicineClassification, MedicineClassificationDto>().ReverseMap();
            CreateMap<AddUpdateMedicineDto, Medicine>()
                .ForMember(dest => dest.Images, act => act.Ignore())
                .ForMember(dest => dest.Id, act => act.Ignore());


            CreateMap<MedicalRecord, MedicalRecordDto>()
                .ReverseMap();
            CreateMap<Doctor, DoctorDto>().ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
            CreateMap<Patient, PatientDto>().ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
            CreateMap<AddUpdateMedicalRecordDto, MedicalRecord>()
                .ForMember(dest => dest.Id, act => act.Ignore());


            CreateMap<Prescription, PrescriptionDto>().ReverseMap();
            CreateMap<PrescriptionDetails, PrescriptionDetailsDto>().ReverseMap();
            CreateMap<AddUpdatePrescriptionDto, Prescription>()
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.Signature, act => act.Ignore());
                
        }
    }
}

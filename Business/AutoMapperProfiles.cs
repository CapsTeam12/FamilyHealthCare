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
            CreateMap<Doctor, DoctorDetailsDto>()
                .ReverseMap();
            CreateMap<Pharmacy, PharmacyDetailsDto>()
                .ReverseMap();
            CreateMap<User, PatientDetailsDto>()
                .ReverseMap();
            CreateMap<MedicineClassification, CategoriesDetailsDto>()
                .ForMember(x => x.CateName, opt => opt.MapFrom(m => m.ClassificationName))
                .ReverseMap();
            CreateMap<Specialities, SpecialitiesDetailsDto>()
                .ReverseMap();

        }
    }
}

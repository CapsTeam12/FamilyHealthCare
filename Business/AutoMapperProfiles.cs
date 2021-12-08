using Contract.DTOs;
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
        }
    }
}

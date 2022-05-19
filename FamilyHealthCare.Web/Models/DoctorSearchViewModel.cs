using Contract.DTOs.ManagementService;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Models
{
    public class DoctorSearchViewModel
    {
        public List<SpecialitiesDetailsDto> Specialities { get; set; }
        public List<DoctorDetailsDto> Doctors { get; set; }
    }
}

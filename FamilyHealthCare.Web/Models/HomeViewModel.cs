using Contract.DTOs.ManagementService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Web.Models
{
    public class HomeViewModel
    {
        public List<DoctorDetailsDto> Doctors { get; set; } 
    }
}

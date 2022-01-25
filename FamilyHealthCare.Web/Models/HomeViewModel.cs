using Contract.DTOs.ManagementService;
using FamilyHealthCare.Customer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Web.Models
{
    public class HomeViewModel : BaseViewModel
    {
        public List<DoctorDetailsDto> Doctors { get; set; }
    }
}

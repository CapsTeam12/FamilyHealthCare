using Contract.DTOs.ManagementService;
using Contract.DTOs.PrescriptionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Pharmacy.Models
{
    public class PrescriptionViewModel
    {
        public PharmacyDetailsDto PharmacyDetails { get; set; }
        public IEnumerable<PrescriptionDto> Prescriptions { get; set; }
    }
}

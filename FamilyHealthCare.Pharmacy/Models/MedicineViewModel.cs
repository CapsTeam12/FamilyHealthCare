using Contract.DTOs.ManagementService;
using Contract.DTOs.MedicineService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Pharmacy.Models
{
    public class MedicineViewModel
    {
        public IEnumerable<MedicineDto> Medicines { get; set; }
        public List<CategoriesDetailsDto> Categories { get; set; }
        public PharmacyDetailsDto Pharmacy { get; set; }
    }
}

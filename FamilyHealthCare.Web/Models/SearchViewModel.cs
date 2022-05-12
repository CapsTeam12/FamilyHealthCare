using Contract.DTOs.ManagementService;
using Contract.DTOs.MedicineService;
using Contract.DTOs.SearchService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Models
{
    public class SearchViewModel
    {
        public List<SearchMedicineDto> SearchMedicineDtos { get; set; }
        public List<FilterCate> FilterCates { get; set; }
        public PharmacyDetailsDto Pharmacy { get; set; } 
    }
}

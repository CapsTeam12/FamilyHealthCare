using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.SearchService
{
    public class SearchPharmacyDto
    {
        public string PharmacyFullName { get; set; }
        public string Certifications { get; set; }
    }
}

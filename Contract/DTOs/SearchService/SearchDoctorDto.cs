using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.SearchService
{
    public class SearchDoctorDto
    {
        public string DoctorFullName { get; set; }
        public string Certifications { get; set; }
    }
}

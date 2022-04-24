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
        public List<int?> Gender { get; set; }
        public List<string> Specialities { get; set; }
    }
}

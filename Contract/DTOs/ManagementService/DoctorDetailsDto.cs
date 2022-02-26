using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.ManagementService
{
    public class DoctorDetailsDto
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string FullName { get; set; }
        public string Specialities { get; set; }
        public string Email { get; set; }
    }
}

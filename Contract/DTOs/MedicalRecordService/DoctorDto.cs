using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.MedicalRecordService
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public string AccountId { get; set; }
        public string Certifications { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public int Languages { get; set; }
        public string Biography { get; set; }
        public string Avatar { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.MedicalRecordService
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string AccountId { get; set; }
        public string FullName { get; set; }
        public int? Gender { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public int? Languages { get; set; }
        public int? PostalCode { get; set; }
        public string Avatar { get; set; }
    }
}

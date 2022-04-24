using Contract.DTOs.ManagementService;
using Contract.DTOs.MedicalRecordService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Doctor.Models
{
    public class MedicalRecordViewModel
    {
        public IEnumerable<MedicalRecordDto> MedicalRecords { get; set; }
        public IEnumerable<PatientDetailsDto> Patients { get; set; }
        public DoctorDetailsDto doctorDetails { get; set; }
    }
}

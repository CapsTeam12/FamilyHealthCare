using Contract.DTOs.MedicalRecordService;
using Contract.DTOs.PrescriptionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Doctor.Models
{
    public class MedicalPrescriptionViewModel
    {
        public AddUpdateMedicalRecordDto medicalRecordDto { get; set; }
        public AddUpdatePrescriptionDto PrescriptionDto { get; set; }
    }
}

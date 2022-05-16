using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.MedicalRecordService
{
    public class AddUpdateMedicalRecordDto
    {
        public int Id { get; set; }
        public string Symptom { get; set; }
        public string Diagnosis { get; set; }
        public string Advice { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ReExamination { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
    }
}

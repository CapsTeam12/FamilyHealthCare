using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Prescription
    {
        public int Id { get; set; }
        public string PrescriptionName { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        [ForeignKey("Pharmacy")]
        public int? PharmacyId { get; set; }
        [ForeignKey("Doctor")]
        public int? DoctorId { get; set; }
        [ForeignKey("MedicalRecord")]
        public int? MedicalRecordId { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public string Notes { get; set; }
        public string Signature { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public string Symptom { get; set; }
        public string Diagnosis { get; set; }
        public string Advice { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ReExamination { get; set; }
        public string PatientId { get; set; }
        public string TherapistId { get; set; }
        public User Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}

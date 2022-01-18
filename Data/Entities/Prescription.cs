using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Prescription
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int PharmacyId { get; set; }
        public Patient Patient { get; set; }
        public Pharmacy Pharmacy { get; set; }
    }
}

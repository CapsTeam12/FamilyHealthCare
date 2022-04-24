using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Experience
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public string HospitalName { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string Designation { get; set; }
    }
}

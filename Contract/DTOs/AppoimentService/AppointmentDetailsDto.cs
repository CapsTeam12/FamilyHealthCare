using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs
{
    public class AppointmentDetailsDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public string TherapistFullName { get; set; }
    }
}

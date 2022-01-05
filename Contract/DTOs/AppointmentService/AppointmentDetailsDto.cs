using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs
{
    public class AppointmentDetailsDto
    {
        public string Id { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public string TherapistFullName { get; set; }
        public int Status { get; set; }
    }
}

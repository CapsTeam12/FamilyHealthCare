using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs
{
    public class AppointmentCreateDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public string TherapistId { get; set; }
        public int Status { get; set; }
    }
}

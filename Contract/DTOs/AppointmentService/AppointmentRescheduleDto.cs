using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.AppoimentService
{
    public class AppointmentRescheduleDto
    {
        public string UserId { get; set; }
        public string TherapistId { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}

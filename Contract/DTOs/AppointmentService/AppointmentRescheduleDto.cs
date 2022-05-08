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
        public string AccountId { get; set; }
        public int PatientId { get; set; }
        public int TherapistId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}

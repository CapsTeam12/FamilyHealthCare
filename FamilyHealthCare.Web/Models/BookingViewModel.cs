using Contract.DTOs.ManagementService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Models
{
    public class BookingViewModel
    {
        public int therapistId { get; set; }
        public string accountDoctorId { get; set; }
        public string doctorName { get; set; }
        public string doctorEmail { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public DoctorDetailsDto doctorDetails { get; set; }



    }
}

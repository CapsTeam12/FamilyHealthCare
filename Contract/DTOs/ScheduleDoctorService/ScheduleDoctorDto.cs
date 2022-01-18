using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.ScheduleDoctorService
{
    public class ScheduleDoctorDto
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public DateTime Date { get; set; }
        public string AccountId { get; set; }
        public bool IsBooking { get; set; }
    }
}

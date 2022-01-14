using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.ScheduleDoctorService
{
    public class ScheduleDoctorCreateDto
    {
        public int ShiftId { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public bool IsBooking { get; set; }

        public int[] ShiftsId { get; set; }
    }
}

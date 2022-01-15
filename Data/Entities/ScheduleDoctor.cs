using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ScheduleDoctor
    {
        public int Id { get; set; }
        public int ShiftId { get; set; }
        public Shift Shift { get; set; }
        public DateTime Date { get; set; }
        public string AccountId { get; set; }
        public User User { get; set; }
        public bool IsBooking { get; set; }

    }
}

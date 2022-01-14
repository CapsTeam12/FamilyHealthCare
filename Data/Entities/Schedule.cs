using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public string AppointmentId { get; set; }
        public string UserId { get; set; }
        public string Eventname { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public User User { get; set; }

    }
}

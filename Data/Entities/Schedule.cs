using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public string AppointmentId { get; set; }
        [ForeignKey("User")]
        public string AccountId { get; set; }
        public string Eventname { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public User User { get; set; }
        public string Join_Url { get; set; }
        public string Start_Url { get; set; }

    }
}

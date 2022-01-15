using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Shift
    {
        public int Id { get; set; }
        public string TimeSlot { get; set; }

        public ICollection<ScheduleDoctor> ScheduleDoctors { get; set; }
    }
}

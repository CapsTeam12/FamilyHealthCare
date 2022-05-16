using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Meeting
    {
        public string Id { get; set; }

        public string Topic { get; set; }

        public string Agenda { get; set; }

        public DateTime StartTime { get; set; }

        public int Duration { get; set; }
    }
}

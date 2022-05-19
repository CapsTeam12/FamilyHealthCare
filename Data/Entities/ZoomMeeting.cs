﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ZoomMeeting
    {
        public string Id { get; set; }

        public string Topic { get; set; }

        public string Agenda { get; set; }

        public DateTime Start_Time { get; set; }

        public int Duration { get; set; }

        public string TimeZone { get; set; }

        public string Join_Url { get; set; }

        public string Start_Url { get; set; }
    }
}

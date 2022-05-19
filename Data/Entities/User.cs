﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class User : IdentityUser
    {
        public User() : base()
        {
        }
        public bool? IsActive { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<ScheduleDoctor> ScheduleDoctors { get; set; }
    }
}

using Contract.DTOs.ScheduleDoctorService;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Doctor.Models
{
    public class ScheduleViewModel
    {
        public List<Shift> Shifts { get; set; }
        public List<ScheduleDoctorDto> ScheduleDoctors { get; set; }
    }
}

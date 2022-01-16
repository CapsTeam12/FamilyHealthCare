using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Doctor.Controllers
{
    public class ScheduleTiming : Controller
    {
        public IActionResult ScheduleTimings()
        {
            return View();
        }
    }
}

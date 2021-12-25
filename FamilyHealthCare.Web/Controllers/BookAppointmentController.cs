using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Web.Controllers
{
    public class BookAppointmentController : Controller
    {
        public IActionResult BookAppointment()
        {
            return View();
        }
    }
}

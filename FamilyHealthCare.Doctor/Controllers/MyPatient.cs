using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    public class MyPatient : Controller
    {
        public IActionResult MyPatients()
        {
            return View();
        }
        public IActionResult MyPatientProfile()
        {
            return View();
        }
    }
}

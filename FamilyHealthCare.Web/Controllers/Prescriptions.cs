using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    public class Prescriptions : Controller
    {
        public IActionResult Prescription()
        {
            return View();
        }
        public IActionResult AddPrescription()
        {
            return View();
        }
        public IActionResult EditPrescription()
        {
            return View();
        }
        public IActionResult DetailPrescription()
        {
            return View();
        }

    }
}

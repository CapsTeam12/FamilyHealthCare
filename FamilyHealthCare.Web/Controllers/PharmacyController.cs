using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    public class PharmacyController : Controller
    {
        public IActionResult Medicine()
        {
            return View();
        }
        public IActionResult AddMedicine()
        {
            return View();
        }
    }
}

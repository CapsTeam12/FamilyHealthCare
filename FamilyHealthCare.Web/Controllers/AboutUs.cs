using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    public class AboutUs : Controller
    {
        public IActionResult Aboutus()
        {
            return View();
        }
    }
}

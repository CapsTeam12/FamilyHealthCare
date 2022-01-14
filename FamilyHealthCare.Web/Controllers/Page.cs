using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    public class Page : Controller
    {
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult AbouttUs()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
    }
}

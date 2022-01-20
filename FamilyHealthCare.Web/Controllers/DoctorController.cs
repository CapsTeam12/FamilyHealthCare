using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        {
            return View();
        }
        public IActionResult Map()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.WebAdmin.Controllers
{
    public class SpecialitiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

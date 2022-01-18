using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Customer.Controllers
{
    public class MedicalRecords : Controller
    {
        public IActionResult MedicalRecord()
        {
            return View();
        }
        public IActionResult MedicalRecordDetails()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IManagementService
    {
        public Task<IActionResult> GetPatientsAsync();
        public Task<IActionResult> GetDoctorsAsync();
        public Task<IActionResult> GetPharmaciesAsync();
        public Task<IActionResult> GetSpecialitiesAsync();
        public Task<IActionResult> GetCategoriestAsync();
    }
}

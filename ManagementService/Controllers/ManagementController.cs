using Business.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly IManagementService _managementService;

        public ManagementController(IManagementService managementService)
        {
            _managementService = managementService;
        }
        // GET: api/<ManagementController>
        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctorsAsync()
        {
            var doctors = await _managementService.GetDoctorsAsync();
            return Ok(doctors);
        }

        [HttpGet("patients")]
        public async Task<IActionResult> GetPatientsAsync()
        {
            var patients = await _managementService.GetPatientsAsync();
            return Ok(patients);
        }

        [HttpGet("pharmacies")]
        public async Task<IActionResult> GetPharmacysAsync()
        {
            var pharmacies = await _managementService.GetPharmaciesAsync();
            return Ok(pharmacies);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            var categories = await _managementService.GetCategoriestAsync();
            return Ok(categories);
        }

        [HttpGet("specialities")]
        public async Task<IActionResult> GetSpecialitiesAsync()
        {
            var specialities = await _managementService.GetSpecialitiesAsync();
            return Ok(specialities);
        }

        // GET api/<ManagementController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ManagementController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ManagementController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ManagementController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

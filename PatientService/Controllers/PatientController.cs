using Business.IServices;
using Contract.DTOs.PartnerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PatientService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IParnerService _partnerService;

        public PatientController(IParnerService parnerService)
        {
            _partnerService = parnerService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DoctorRegister([FromForm] DoctorRegisterDto doctorRegisterDto)
        {
            var doctorDto = await _partnerService.DoctorRegister(doctorRegisterDto);
            if (doctorDto != null)
                return Ok(doctorDto);
            return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> PharmacyRegister([FromForm] PharmacyRegisterDto pharmacyRegisterDto)
        {
            var pharmacyDto = await _partnerService.PharmacyRegister(pharmacyRegisterDto);
            if (pharmacyDto != null)
                return Ok(pharmacyDto);
            return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CheckEmailExist([FromForm]string email)
        {
            var resultCheck = await _partnerService.CheckEmailExist(email);
            if (resultCheck)
            {
                return Ok(resultCheck);
            }
            return Ok(resultCheck);
        }

    }
}

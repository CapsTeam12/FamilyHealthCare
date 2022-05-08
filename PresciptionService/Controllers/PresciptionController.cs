using Business.IServices;
using Contract.DTOs.PrescriptionService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresciptionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresciptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PresciptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpGet("[action]/{accountId}")]
        public async Task<IActionResult> GetPrescrionsByDoctor(string accountId)
        {
            var prescriptions = await _prescriptionService.GetPrescriptionsDoctor(accountId);
            if(prescriptions != null)
            {
                return Ok(prescriptions);
            }
            return NotFound();
        }

        [HttpGet("[action]/{accountId}")]
        public async Task<IActionResult> GetPrescrionsByPharmacy(string accountId)
        {
            var prescriptions = await _prescriptionService.GetPrescriptionsPharmacy(accountId);
            if (prescriptions != null)
            {
                return Ok(prescriptions);
            }
            return NotFound();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetPrescrionDetails(int id)
        {
            var prescriptions = await _prescriptionService.GetPrescriptionDetails(id);
            if (prescriptions != null)
            {
                return Ok(prescriptions);
            }
            return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePrescription([FromForm] AddUpdatePrescriptionDto prescriptionDto)
        {
            var prescriptions = await _prescriptionService.AddPrescription(prescriptionDto);
            if(prescriptions != null)
            {
                return Ok(prescriptions);
            }
            return NotFound();
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdatePrescription(int id,[FromForm] AddUpdatePrescriptionDto prescriptionDto)
        {
            var prescriptions = await _prescriptionService.UpdatePrescription(id,prescriptionDto);
            if (prescriptions != null)
            {
                return Ok(prescriptions);
            }
            return NotFound();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescriptions = await _prescriptionService.DeletePrescription(id);
            if (prescriptions != null)
            {
                return Ok(prescriptions);
            }
            return NotFound();
        }

    }
}

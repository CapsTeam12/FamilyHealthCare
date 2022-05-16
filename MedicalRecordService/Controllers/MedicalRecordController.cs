using Business.IServices;
using Contract.DTOs.MedicalRecordService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MedicalRecordService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        // GET: api/<MedicalRecordController>
        [HttpGet("[action]/{accountId}")]
        public async Task<IActionResult> GetMedicalRecords(string accountId)
        {
            var medicalRecord = await _medicalRecordService.GetMedicalRecordsByDoctor(accountId);
            if(medicalRecord != null)
            {
                return Ok(medicalRecord);
            }
            return NotFound();
        }

        // POST api/<MedicalRecordController>
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateMedicalRecord([FromBody] AddUpdateMedicalRecordDto recordDto)
        {
            var medicalRecord = await _medicalRecordService.CreateMedicalRecord(recordDto);
            if (medicalRecord != null)
            {
                return Ok(medicalRecord);
            }
            return NotFound();
        }

        // PUT api/<MedicalRecordController>/5
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateMedicalRecord(int id, [FromBody] AddUpdateMedicalRecordDto recordDto)
        {
            var medicalRecord = await _medicalRecordService.UpdateMedicalRecord(id,recordDto);
            if (medicalRecord != null)
            {
                return Ok(medicalRecord);
            }
            return NotFound();
        }

        // DELETE api/<MedicalRecordController>/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            var medicalRecord = await _medicalRecordService.DeleteMedicalRecord(id);
            if (medicalRecord != null)
            {
                return Ok(medicalRecord);
            }
            return NotFound();
        }
    }
}

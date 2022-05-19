using Business.IServices;
using Contract.DTOs.MedicineService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicineService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetMedicines()
        {
            var medicines = await _medicineService.GetMedicines();
            if(medicines != null)
            {
                return Ok(medicines);
            }
            return NotFound();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetMedicinesByPharmacy(string id)
        {
            var medicines = await _medicineService.GetMedicinesByPharmacyAccountId(id);
            if (medicines != null)
            {
                return Ok(medicines);
            }
            return NotFound();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetMedicinesByPharmacyId(int id)
        {
            var medicines = await _medicineService.GetMedicinesByPharmacyId(id);
            if (medicines != null)
            {
                return Ok(medicines);
            }
            return NotFound();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateMedicine([FromForm]AddUpdateMedicineDto medicineDto)
        {
            var medicines = await _medicineService.CreateMedicine(medicineDto);
            if (medicines != null)
            {
                return Ok(medicines);
            }
            return NotFound();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetMedicineDetails(int id)
        {
            var medicine = await _medicineService.GetMedicineDetails(id);
            if (medicine != null)
            {
                return Ok(medicine);
            }
            return NotFound();
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateMedicine(int id,[FromForm] AddUpdateMedicineDto medicineDto)
        {
            var medicines = await _medicineService.UpdateMedicine(id,medicineDto);
            if (medicines != null)
            {
                return Ok(medicines);
            }
            return NotFound();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> ReturnMedicine(int id)
        {
            var medicines = await _medicineService.ReturnMedicine(id);
            if (medicines != null)
            {
                return Ok(medicines);
            }
            return NotFound();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicines = await _medicineService.DeleteMedicine(id);
            if (medicines != null)
            {
                return Ok(medicines);
            }
            return NotFound();
        }
    }
}

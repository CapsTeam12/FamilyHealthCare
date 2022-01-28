using Contract.DTOs.ManagementService;
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
        public Task<List<PatientDetailsDto>> GetPatientsAsync();
        public Task<List<DoctorDetailsDto>> GetDoctorsAsync();
        public Task<IActionResult> GetPharmaciesAsync();
        public Task<IActionResult> GetSpecialitiesAsync();
        public Task<List<CategoriesDetailsDto>> GetCategoriesAsync();

        public Task<PatientDetailsDto> GetPatientDetailsAsync(string id);
        public Task<IActionResult> GetDoctorDetailsAsync(string id);
        public Task<IActionResult> GetPharmacyDetailsAsync(string id);
        public Task<IActionResult> GetSpecializedDetailsAsync(string id);
        public Task<IActionResult> GetCategoryDetailsAsync(string id);
    }
}

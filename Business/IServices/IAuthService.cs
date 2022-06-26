using Contract.DTOs.AuthService;
using Contract.DTOs.ManagementService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IAuthService 
    {
        public Task<IActionResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        public Task<IActionResult> UpdatePatientProfileAsync(PatientUpdateDto patientDetailsDto);
        public Task<IActionResult> UpdateDoctorProfileAsync(DoctorUpdateDto doctorDetailsDto);
        public Task<IActionResult> UpdatePharmacyProfileAsync(PharmacyUpdateDto pharmacyDetailsDto);
        public Task<IActionResult> UpdateAdminProfileAsync();
    }
}

using Contract.DTOs.AuthService;
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
        public Task<IActionResult> UpdatePatientProfileAsync();
        public Task<IActionResult> UpdateDoctorProfileAsync();
        public Task<IActionResult> UpdatePharmacyProfileAsync();
        public Task<IActionResult> UpdateAdminProfileAsync();
    }
}

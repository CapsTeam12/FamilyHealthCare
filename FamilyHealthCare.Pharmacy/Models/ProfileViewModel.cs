using Contract.DTOs.AuthService;
using Contract.DTOs.ManagementService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyHealthCare.Pharmacy.Models
{
    public class ProfileViewModel
    {
        public PharmacyDetailsDto PharmacyDetails { get; set; }
        public ChangePasswordDto ChangePassword { get; set; }
    }
}

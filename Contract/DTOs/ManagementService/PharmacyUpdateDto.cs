using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.ManagementService
{
    public class PharmacyUpdateDto
    {
        public string AccountId { get; set; }
        [Required]
        public string PharmacyName { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone must be numeric")]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        public IFormFile Avatar { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.ManagementService
{
    public class DoctorUpdateDto
    {
        public string AccountId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public int? Gender { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone must be numeric")]
        public string Phone { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Address { get; set; }
        public IFormFile Avatar { get; set; }
    }
}

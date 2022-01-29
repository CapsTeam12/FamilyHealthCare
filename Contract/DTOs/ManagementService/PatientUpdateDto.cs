using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.ManagementService
{
    public class PatientUpdateDto
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
        [Required]
        public int? Languages { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Postal Code must be numeric")]
        public int? PostalCode { get; set; }
        public IFormFile Avatar { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}

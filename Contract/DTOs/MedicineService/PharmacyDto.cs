using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.MedicineService
{
    public class PharmacyDto
    {
        public int Id { get; set; }
        public string Certifications { get; set; }
        public string PharmacyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Languages { get; set; }
        public string Biography { get; set; }
        public int PostalCode { get; set; }
        public string Avatar { get; set; }
    }
}

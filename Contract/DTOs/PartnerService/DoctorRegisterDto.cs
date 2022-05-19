using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.PartnerService
{
    public class DoctorRegisterDto
    {
        public IFormFile Certifications { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int SpecializedId { get; set; }
        public int Gender { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public int Languages { get; set; }
        public string Biography { get; set; }
        public IFormFile Avatar { get; set; }
        public List<ExperiencesDto> Experiences { get; set; }

        public DoctorRegisterDto()
        {
            this.Experiences = new List<ExperiencesDto>();
        }

    }
}

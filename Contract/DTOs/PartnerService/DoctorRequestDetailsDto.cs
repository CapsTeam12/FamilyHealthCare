using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.PartnerService
{
    public class DoctorRequestDetailsDto
    {
        public int Id { get; set; }
        public string Certifications { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Specialized { get; set; }
        public int Gender { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public int Languages { get; set; }
        public string Biography { get; set; }
        public string Avatar { get; set; }
        public List<ExperiencesDto> Experiences { get; set; }

        public DoctorRequestDetailsDto()
        {
            this.Experiences = new List<ExperiencesDto>();
        }
    }
}

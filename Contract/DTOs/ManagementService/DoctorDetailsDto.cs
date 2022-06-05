using Contract.DTOs.PartnerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.ManagementService
{
    public class DoctorDetailsDto
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string FullName { get; set; }
        public string Specialities { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }
        public string Biography { get; set; }
        public int Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<ExperiencesDto> Experiences { get; set; }
        public DoctorDetailsDto()
        {
            this.Experiences = new List<ExperiencesDto>();
        }
    }
}

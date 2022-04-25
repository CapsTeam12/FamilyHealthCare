﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.PartnerService
{
    public class PharmacyRequestDetailsDto
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
        public List<AwardsDto> Awards { get; set; }
        public string Avatar { get; set; }
        public PharmacyRequestDetailsDto()
        {
            this.Awards = new List<AwardsDto>();
        }
    }
}
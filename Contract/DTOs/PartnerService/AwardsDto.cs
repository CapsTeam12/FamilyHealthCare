using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.PartnerService
{
    public class AwardsDto
    {
        public int PharmacyId { get; set; }
        //public PharmacyRegisterDto Pharmacy { get; set; }
        public string Award { get; set; }
        public string Year { get; set; }
    }
}

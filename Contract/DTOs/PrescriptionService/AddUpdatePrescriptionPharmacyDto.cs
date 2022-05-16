using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.PrescriptionService
{
    public class AddUpdatePrescriptionPharmacyDto
    {
        public int Id { get; set; }
        public string PrescriptionName { get; set; }
        public DateTime Date { get; set; }
        public int PharmacyId { get; set; }
        public List<PrescriptionDetailsDto> PrescriptionDetailsDtos { get; set; }
        public string Notes { get; set; }
        public IFormFile Signature { get; set; }
        public AddUpdatePrescriptionPharmacyDto()
        {
            this.PrescriptionDetailsDtos = new List<PrescriptionDetailsDto>();
        }
    }
}

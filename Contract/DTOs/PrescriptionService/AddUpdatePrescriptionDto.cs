using Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.PrescriptionService
{
    public class AddUpdatePrescriptionDto
    {
        public int Id {get; set; }
        public string PrescriptionName { get; set; }
        public DateTime Date { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int MedicalRecordId { get; set; }
        public List<PrescriptionDetailsDto> PrescriptionDetailsDtos { get; set; }
        public string Notes { get; set; }
        public IFormFile Signature { get; set; }
        public AddUpdatePrescriptionDto()
        {
            this.PrescriptionDetailsDtos = new List<PrescriptionDetailsDto>();
        }
    }
}

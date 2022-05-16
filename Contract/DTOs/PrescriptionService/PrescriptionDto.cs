using Contract.DTOs.MedicalRecordService;
using Contract.DTOs.MedicineService;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.PrescriptionService
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public PatientDto Patient { get; set; }
        public int PharmacyId { get; set; }
        public PharmacyDto Pharmacy { get; set; }
        public int DoctorId { get; set; }
        public DoctorDto Doctor { get; set; }
        public int MedicalRecordId { get; set; }
        public MedicalRecordDto MedicalRecord { get; set; }
        public string PrescriptionName { get; set; }
        public DateTime Date { get; set; }
        public List<PrescriptionDetailsDto> PrescriptionDetailsDtos { get; set; }
        public string Notes { get; set; }
        public string Signature { get; set; }
        public PrescriptionDto()
        {
            this.PrescriptionDetailsDtos = new List<PrescriptionDetailsDto>();
        }
    }
}

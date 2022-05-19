using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.MedicineService
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public DateTime ImportDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Ingredients { get; set; }
        public string Direction { get; set; }
        public string Images { get; set; }
        public int Status { get; set; }
        public int ClassificationID { get; set; }
        public MedicineClassificationDto MedicineClass { get; set; }
        public int PharmacyId { get; set; }
        public PharmacyDto Pharmacy { get; set; }
    }
}

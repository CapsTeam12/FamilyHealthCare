using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Medicine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [ForeignKey("MedicineClass")]
        public int ClassificationID { get; set; }
        public MedicineClassification MedicineClass { get; set; }
        [ForeignKey("Pharmacy")]
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class PrescriptionDetails
    {
        public int Id { get; set; }
        [ForeignKey("Prescription")]
        public int? PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }
        [ForeignKey("Medicine")]
        public int? MedicineId { get; set; }
        public Medicine Medicine { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
    }
}

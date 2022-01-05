using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class PrescriptionDetails
    {
        public int Id { get; set; }
        public string PrecriptionId { get; set; }
        public Prescription Prescription { get; set; }
        public string MedicineId { get; set; }
        public Medicine Medicine { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; }
    }
}

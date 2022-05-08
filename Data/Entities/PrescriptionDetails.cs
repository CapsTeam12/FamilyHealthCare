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
        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public int Days { get; set; }
        public int Time { get; set; }
    }
}

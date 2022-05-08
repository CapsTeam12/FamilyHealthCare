using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.PrescriptionService
{
    public class PrescriptionDetailsDto
    {
        public int PrescriptionId { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public int Days { get; set; }
        public int[] Time { get; set; }        
    }
}

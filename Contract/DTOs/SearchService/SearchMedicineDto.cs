using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.SearchService
{
    public class SearchMedicineDto
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Direction { get; set; }
        public string Images { get; set; }
        public string ClassificationName { get; set; }
    }
}

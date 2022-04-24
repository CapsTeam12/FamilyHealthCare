using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Awards
    {
        public int Id { get; set; }
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public string Award { get; set; }
        public string Year { get; set; }

    }
}

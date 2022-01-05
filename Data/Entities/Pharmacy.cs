using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Pharmacy
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Certifications { get; set; }

    }
}

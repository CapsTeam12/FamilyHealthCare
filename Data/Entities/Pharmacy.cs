using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Pharmacy
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string AccountId { get; set; }
        public User User { get; set; }
        public string Certifications { get; set; }
        public string PharmacyName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int Languages { get; set; }
        public string Biography { get; set; }
        public int PostalCode { get; set; }
    }

}

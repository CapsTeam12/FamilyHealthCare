using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string AccountId { get; set; }
        public User User { get; set; }
        public string FullName { get; set; }
        public int? Gender { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public int? Languages { get; set; }
        public int? PostalCode { get; set; }
        //public string Avatar { get; set; }
    }
}

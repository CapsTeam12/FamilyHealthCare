using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string AccountId { get; set; }
        public User User { get; set; }
        public string Certifications { get; set; }
        public string FullName { get; set; }
        [ForeignKey("Specialized")]
        public int SpecializedId { get; set; }
        public Specialities Specialized { get; set; }
        public int Gender { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public int Languages { get; set; }
        public string Biography { get; set; }
        public string Avatar { get; set; }
    }
}

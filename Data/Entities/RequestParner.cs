using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class RequestParner
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int Role { get; set; }
        public string Certifications { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}

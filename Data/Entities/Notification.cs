using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserID { get; set; }  
        public User User { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}

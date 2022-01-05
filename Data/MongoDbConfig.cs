using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class MongoDbConfig
    {
        public string Database_Name { get; set; }
        public string Appointments_Collection_Name { get; set; }
        public string Connection_String { get; set; }
    }
}

using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.HealthCheck
{
    public class HealthCheckDto 
    { 
            public double Weight { get; set; }
            public double Height { get; set; }
            public int Age { get; set; }
            public string BloodPressure { get; set; }
            public int HeartRate { get; set; }
            public double BMI { get; set; }
            public DateTime Date { get; set; }
            public string UserId { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.PartnerService
{
    //[ModelBinder(BinderType = typeof(MetadataValueModelBinder))]
    public class ExperiencesDto
    {
        public int DoctorId { get; set; }
        //public DoctorRegisterDto Doctor { get; set; }
        public string HospitalName { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string Designation { get; set; }
    }
}

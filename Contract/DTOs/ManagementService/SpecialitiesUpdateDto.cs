﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.ManagementService
{
    public class SpecialitiesUpdateDto
    {
        public int Id { get; set; }
        public string SpecializedName { get; set; }
        public IFormFile Image { get; set; }
    }
}
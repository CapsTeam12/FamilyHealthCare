using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.SearchService
{
    public class SearchCategoryDto
    {
        public string Search { get; set; }
        public List<string> FilterCates { get; set; }
    }
}

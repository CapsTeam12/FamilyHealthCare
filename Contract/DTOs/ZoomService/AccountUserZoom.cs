using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.ZoomService
{
    [JsonObject]
    public class AccountUsersZoom
    {
        [JsonProperty("page_count")]
        public Int32 PageCount { get; set; }

        [JsonProperty("page_number")]
        public Int32 PageNumber { get; set; }

        [JsonProperty("page_size")]
        public Int32 PageSize { get; set; }

        [JsonProperty("total_records")]
        public Int32 TotalRecords { get; set; }

        [JsonProperty("users")]
        public List<UserZoom> Users { get; set; }
    }
}

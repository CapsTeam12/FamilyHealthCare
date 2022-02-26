using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.ZoomService
{
    public class UserZoom
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("type")]
        public Int32 Type { get; set; }

        [JsonProperty("pmi")]
        public string Pmi { get; set; }

        [JsonProperty("timezone")]
        public string TimeZone { get; set; }

        [JsonProperty("verified")]
        public Int32 Verified { get; set; }

        [JsonProperty("dept")]
        public string Department { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("last_login_time")]
        public DateTime LastLoginTime { get; set; }

        [JsonProperty("last_client_version")]
        public string LastClientVersion { get; set; }
    }
}

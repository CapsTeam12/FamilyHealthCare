using Business.IServices;
using Contract.DTOs.ZoomService;
using Data.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ZoomService : IZoomService
    {
        private const string ApiKey = "6U-aE2JbRAKHbntPNrUBZQ";
        private const string ApiSecret = "85krCB9BYoa7liU3c6rlKNuCyS73za8LzH4K";

        public async Task<IEnumerable<ZoomMeeting>> AllMeetings(string email)
        {
            string token = new ZoomToken(ApiKey, ApiSecret).Token;

            RestClient restClient = new RestClient($"https://api.zoom.us/v2/users/{email}/meetings");
            RestRequest restRequest = new RestRequest();

            restRequest.AddHeader("Authorization", "Bearer " + token);
            var response = await restClient.GetAsync(restRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var zoomMeetings = JObject.Parse(response.Content)["meetings"].ToObject<IEnumerable<ZoomMeeting>>();
                foreach (ZoomMeeting meeting in zoomMeetings)
                {
                    meeting.Start_Time = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(meeting.Start_Time.Ticks, DateTimeKind.Unspecified), TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
                }
                return zoomMeetings;
            }
            return null;
        }

        public async Task<ZoomMeeting> Meeting(string Id)
        {
            string token = new ZoomToken(ApiKey, ApiSecret).Token;

            RestClient restClient = new RestClient($"https://api.zoom.us/v2/meetings/{Id}");
            RestRequest restRequest = new RestRequest();

            restRequest.AddHeader("Authorization", "Bearer " + token);

            var response = await restClient.GetAsync(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var zoomMeeting = JObject.Parse(response.Content).ToObject<ZoomMeeting>();
                return zoomMeeting;
            }
            return null;
        }

        public async Task<ZoomMeeting> CreateMeeting(Meeting meeting, string email)
        {
            string token = new ZoomToken(ApiKey, ApiSecret).Token;

            RestClient restClient = new RestClient($"https://api.zoom.us/v2/users/{email}/meetings");
            RestRequest restRequest = new RestRequest();

            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Authorization", string.Format("Bearer {0}", token));
            restRequest.AddJsonBody(new
            {
                topic = meeting.Topic,
                agenda = meeting.Agenda,
                start_time = meeting.StartTime.ToString("yyyy-MM-dd'T'HH:mm:ss"),
                duration = meeting.Duration
            }, "application/json");

            var response = await restClient.PostAsync(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                ZoomMeeting zoomMeeting = JsonConvert.DeserializeObject<ZoomMeeting>(response.Content);
                return zoomMeeting;
            }
            return null;
        }

        public async Task<bool> UpdateMeeting(string identifier, Meeting meeting)
        {
            string token = new ZoomToken(ApiKey, ApiSecret).Token;

            RestClient restClient = new RestClient($"https://api.zoom.us/v2/meetings/{identifier}");
            RestRequest restRequest = new RestRequest();

            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Authorization", "Bearer " + token);
            //restRequest.AddParameter("application/json", model, ParameterType.RequestBody);
            restRequest.AddJsonBody(new
            {
                topic = meeting.Topic,
                agenda = meeting.Agenda,
                start_time = meeting.StartTime.ToString("yyyy-MM-dd'T'HH:mm:ss"),
                duration = meeting.Duration
            }, "application/json");

            var response = await restClient.PatchAsync(restRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteMeeting(string identifier)
        {
            string token = new ZoomToken(ApiKey, ApiSecret).Token;
            RestClient restClient = new RestClient($"https://api.zoom.us/v2/meetings/{identifier}");
            RestRequest restRequest = new RestRequest();

            restRequest.AddHeader("Authorization", "Bearer " + token);

            var response = await restClient.DeleteAsync(restRequest);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        public async Task<UserZoomDetail> CreateUser(UserZoomDetail userZoom)
        {
            string token = new ZoomToken(ApiKey, ApiSecret).Token;

            RestClient restClient = new RestClient("https://api.zoom.us/v2/users");
            RestRequest restRequest = new RestRequest();

            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Authorization", string.Format("Bearer {0}", token));
            restRequest.AddJsonBody(new
            {
                action = "create",
                user_info = userZoom
            }, "application/json");

            var response = await restClient.PostAsync(restRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var userDetail = JsonConvert.DeserializeObject<UserZoomDetail>(response.Content);
                return userDetail;
            }
            return null;
        }

        public async Task<IEnumerable<UserZoom>> GetUserDetails()
        {
            string token = new ZoomToken(ApiKey, ApiSecret).Token;
            RestClient restClient = new RestClient("https://api.zoom.us/v2/users?status=active&page_size=30&page_number=1");
            RestRequest request = new RestRequest();

            request.AddHeader("content-type", "application/json");
            request.AddHeader("Authorization", string.Format("Bearer {0}", token));

            var response = await restClient.GetAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var listUsers = JObject.Parse(response.Content)["users"].ToObject<IEnumerable<UserZoom>>();
                return listUsers;
            }
            return null;
        }


    }
}

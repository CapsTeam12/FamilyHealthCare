using Contract.DTOs.NotificationServiceDtos;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Business
{
    public class NotificationHelper
    {
        private string _baseAddress = "http://localhost:52409";
        private string _createPath = "/notification/create";
        private string _markAsReadPath = "/notification/mark-as-read/{0}";
        private HttpClient _client;

        public NotificationHelper()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task CallApiCreateNotification(NotificationCreateDto notificationCreateDto)
        {
            await _client.PostAsJsonAsync(_createPath, notificationCreateDto);
        }

        public async Task CallApiMarkAsRead(int notificationId)
        {
            await _client.GetAsync(string.Format(_markAsReadPath, notificationId));
        }
    }
}

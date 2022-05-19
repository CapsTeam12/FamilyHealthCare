using Contract.DTOs.NotificationServiceDtos;
using FamilyHealthCare.SharedLibrary.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyHealthCare.SharedLibrary
{
    public class NotificationHelperClient : INotificationHelperClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContext;
        public NotificationHelperClient(IHttpClientFactory clientFactory, IHttpContextAccessor httpContext)
        {
            _clientFactory = clientFactory;
            _httpContext = httpContext;
        }
        public async Task<List<NotificationListDto>> GetNotification()
        {
            var userId = _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var httpClient = _clientFactory.CreateClient(ServiceConstants.NOTIFICATION_NAMED_CLIENT);
            var response = await httpClient.GetAsync($"{EndpointConstants.NotificationService.NOTIFICATION}/{userId}");
            var data = new List<NotificationListDto>();
            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsAsync<List<NotificationListDto>>();
            }
            return data;
        }
    }
}

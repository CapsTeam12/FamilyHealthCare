using Business.IServices;
using Contract.DTOs.NotificationServiceDtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{id}")]
        public async Task<List<NotificationListDto>> GetNotificationList(string id)
        {
            return await _notificationService.GetNotificationListAsync(id);
        }
    }
}

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

        [HttpPost("create")]
        public async Task<NotificationListDto> CreateNotification(NotificationCreateDto notificationCreateDto)
        {
            return await _notificationService.CreateNotificationAsync(notificationCreateDto);
        }

        [HttpGet("mark-as-read/{notificationId}")]
        public async Task<NotificationListDto> MarkAsRead(int notificationId)
        {
            return await _notificationService.MarkNotificationAsReadAsync(notificationId);
        }
    }
}

using Contract.DTOs.NotificationServiceDtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface INotificationService
    {
        public Task<List<NotificationListDto>> GetNotificationListAsync(string id);
        public Task<NewNotificationDto> CreateNotificationAsync(NotificationCreateDto notificationCreateDto);
        public Task<NewNotificationDto> MarkNotificationAsReadAsync(int notificationId);
    }
}

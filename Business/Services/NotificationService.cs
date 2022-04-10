using Business.IServices;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;
using System;
using System.Threading.Tasks;

namespace Business.Services
{
    public class NotificationService : ControllerBase, INotificationService
    {
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IBaseRepository<Notification> notificationRepository,
            IHubContext<NotificationHub> hubContext)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }

        public Task<IActionResult> CreateNotification()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> MarkNotificationAsRead()
        {
            throw new NotImplementedException();
        }
    }
}

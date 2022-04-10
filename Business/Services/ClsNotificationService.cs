using Business.IServices;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Business.Hubs;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Contract.DTOs.NotificationServiceDtos;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class ClsNotificationService : ControllerBase, INotificationService
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ClsNotificationService(IMapper mapper,
            IBaseRepository<Notification> notificationRepository,
            IHubContext<NotificationHub> hubContext)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }

        public async Task<List<NotificationListDto>> GetNotificationListAsync(string id)
        {
            var notifications = await _notificationRepository.Entities.Where(n => n.UserID == id).ToListAsync();
            var notificationDtos = _mapper.Map<List<NotificationListDto>>(notifications);
            return notificationDtos;
        }

        public Task<IActionResult> CreateNotificationAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> MarkNotificationAsReadAsync()
        {
            throw new NotImplementedException();
        }
    }
}

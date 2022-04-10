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
            var notifications = await _notificationRepository
                .Entities.Where(n => n.UserID == id).OrderByDescending(n => n.Time).ToListAsync();
            var notificationDtos = _mapper.Map<List<NotificationListDto>>(notifications);
            return notificationDtos;
        }

        public async Task<NotificationListDto> CreateNotificationAsync(NotificationCreateDto notificationCreateDto)
        {
            var newNotification = new Notification
            {
                UserID = notificationCreateDto.UserID,
                Content = notificationCreateDto.Content,
                AvatarSender = notificationCreateDto.AvatarSender,
                IsRead = false,
                Time = DateTime.Now
            };

            await _notificationRepository.Create(newNotification);
            var newNotificationDto = _mapper.Map<NotificationListDto>(newNotification);

            //await _hubContext.Clients.User(notificationCreateDto.UserID.ToString())
            //                .SendAsync("SendNotification", newNotificationDto);

            return newNotificationDto;
        }

        public async Task<IActionResult> MarkNotificationAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetById(notificationId);
            if(notification == null)
            {
                return NotFound();
            }
            notification.IsRead = true;
            var notificationDto = _mapper.Map<NotificationListDto>(notification);

            //await _hubContext.Clients.User(notification.UserID.ToString())
            //                .SendAsync("MarkNotificationAsRead", notificationDto);

            return Ok(notificationDto);
        }
    }
}

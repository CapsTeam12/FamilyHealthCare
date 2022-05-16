﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.DTOs.NotificationServiceDtos
{
    public class NewNotificationDto
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }
        public DateTime ActualTime { get; set; }
        public bool IsRead { get; set; }
        public string AvatarSender { get; set; }
    }
}
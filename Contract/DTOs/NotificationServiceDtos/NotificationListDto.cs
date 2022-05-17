using System;

namespace Contract.DTOs.NotificationServiceDtos
{
    public class NotificationListDto
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }
        public bool IsRead { get; set; }
        public string AvatarSender { get; set; }
    }
}

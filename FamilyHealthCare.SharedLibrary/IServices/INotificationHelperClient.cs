using Contract.DTOs.NotificationServiceDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamilyHealthCare.SharedLibrary.IServices
{
    public interface INotificationHelperClient
    {
        public Task<List<NotificationListDto>> GetNotification();
    }
}

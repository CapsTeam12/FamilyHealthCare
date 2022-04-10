using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Business.Hubs
{
    public class NotificationHub : Hub
    {
        //public async Task SendNotification(NotificationListDto notificationListDto)
        //{
        //    await Clients.All.SendAsync("SendNotification", notificationListDto);
        //}

        //public async Task MarkNotificationAsRead(NotificationListDto notificationListDto)
        //{
        //    await Clients.All.SendAsync("MarkNotificationAsRead", notificationListDto);
        //}
    }
}

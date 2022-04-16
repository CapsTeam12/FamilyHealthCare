using FamilyHealthCare.SharedLibrary.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FamilyHealthCare.Doctor.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly INotificationHelperClient _notificaitonHelperClient;

        public NotificationViewComponent(INotificationHelperClient notificaitonHelperClient)
        {
            _notificaitonHelperClient = notificaitonHelperClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            await _notificaitonHelperClient.GetNotification();
            return await Task.FromResult((IViewComponentResult)View());
        }
    }
}

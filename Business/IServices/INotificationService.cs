using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface INotificationService
    {
        public Task<IActionResult> CreateNotification();
        public Task<IActionResult> MarkNotificationAsRead();
    }
}

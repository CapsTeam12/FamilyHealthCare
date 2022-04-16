using System.Threading.Tasks;

namespace FamilyHealthCare.SharedLibrary.IServices
{
    public interface INotificationHelperClient
    {
        public Task GetNotification();
    }
}

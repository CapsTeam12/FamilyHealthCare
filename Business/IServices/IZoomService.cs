using Contract.DTOs.ZoomService;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IZoomService
    {
        Task<IEnumerable<UserZoom>> GetUserDetails();
        Task<UserZoomDetail> CreateUser(UserZoomDetail userZoom);
        Task<ZoomMeeting> CreateMeeting(Meeting meeting,string email);
        Task<IEnumerable<ZoomMeeting>> AllMeetings(string email);
        Task<ZoomMeeting> Meeting(string Id);


    }
}

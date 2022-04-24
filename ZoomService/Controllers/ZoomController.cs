using Business.IServices;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZoomService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoomController : ControllerBase
    {
        private readonly IZoomService _zoomService;

        public ZoomController(IZoomService zoomService)
        {
            _zoomService = zoomService;
        }

        [HttpGet]
        [Route("ListUser")]
        public async Task<IActionResult> GetUserDetails()
        {
            var users = await _zoomService.GetUserDetails();
            if (users != null)
            {
                return Ok(users);
            }
            return NoContent();
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserZoomDetail userZoom)
        {
            var user = await _zoomService.CreateUser(userZoom);
            if (user != null)
            {
                return Ok(user);
            }
            return NoContent();
        }

        [HttpGet]
        [Route("ListMeeting/{email}")]
        public async Task<IActionResult> GetAllMeeting(string email)
        {
            var meetings = await _zoomService.AllMeetings(email);
            if (meetings != null)
            {
                return Ok(meetings);
            }
            return NoContent();
        }

        [HttpGet]
        [Route("Meeting/{id}")]
        public async Task<IActionResult> GetMeeting(string id)
        {
            var meeting = await _zoomService.Meeting(id);
            if (meeting != null)
            {
                return Ok(meeting);
            }
            return NoContent();
        }


        [HttpPost]
        [Route("CreateMeeting/{email}")]
        public async Task<IActionResult> CreateMeeting([FromBody] Meeting meeting, string email)
        {
            var meetingOfUser = await _zoomService.CreateMeeting(meeting, email);
            if (meetingOfUser != null)
            {
                return Ok(meetingOfUser);
            }
            return NoContent();
        }

        [HttpPatch]
        [Route("UpdateMeeting/{identifier}")]
        public async Task<IActionResult> UpdateMeeting([FromBody] Meeting meeting, string identifier)
        {
            var meetingOfUser = await _zoomService.UpdateMeeting(identifier, meeting);
            if (meetingOfUser == true)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("DeleteMeeting/{identifier}")]
        public async Task<IActionResult> DeleteMeeting(string identifier)
        {
            var meeting = await _zoomService.DeleteMeeting(identifier);
            if (meeting == true)
            {
                return NoContent();
            }
            return BadRequest();
        }

    }
}

﻿using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace NotificationService.Hubs
{
    public class NotificationHub : Hub
    {
        //public async Task SendNotification(string user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}
    }
}
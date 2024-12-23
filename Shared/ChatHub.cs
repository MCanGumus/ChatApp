using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Guid userId, string message)
        {
            await Clients.All.SendAsync("chathub", userId, message);
        }
    }
}

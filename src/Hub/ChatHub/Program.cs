using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<Guid, string> ConnectedUsers = new();

    public override Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext()?.Request.Query["userId"];
        if (Guid.TryParse(userId, out var guid))
        {
            ConnectedUsers[guid] = Context.ConnectionId;
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = ConnectedUsers.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
        if (userId != Guid.Empty)
        {
            ConnectedUsers.TryRemove(userId, out _);
        }

        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(Guid toWho, string message)
    {
        if (ConnectedUsers.TryGetValue(toWho, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }
    }
}
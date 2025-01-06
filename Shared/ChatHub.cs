using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class ChatHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> ConnectedUsers = new();

    private async Task UpdateConnectedUsers()
    {
        await Clients.All.SendAsync("GetConnectedUsers", ConnectedUsers);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext()?.Request.Query["userId"];

        ConnectedUsers[userId] = Context.ConnectionId;


        await UpdateConnectedUsers();

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = ConnectedUsers.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

        if (string.IsNullOrEmpty(userId))
        {
            ConnectedUsers.TryRemove(userId, out _);
        }
        await UpdateConnectedUsers();

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string toWho, string userName, string message)
    {
        if (ConnectedUsers.TryGetValue(toWho, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", toWho, userName, message);
        }
    }

    public string GetObtainedClientId()
    {
        return Context.ConnectionId;
    }
}
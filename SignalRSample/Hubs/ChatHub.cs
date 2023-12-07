using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SignalRSample.Hubs;

public class ChatHub : Hub
{
    private readonly ApplicationDbContext _context;

    public ChatHub(ApplicationDbContext context) => _context = context;

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(!string.IsNullOrEmpty(userId))
        {
            var userName = (await _context.Users.FirstOrDefaultAsync(u => u.Id == userId))!.UserName;
            await Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReveiveUserConnected",userId,userName);
            HubConnections.AddUserConnection(userId,Context.ConnectionId);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(HubConnections.HasUserConnection(userId,Context.ConnectionId))
        {
            var userConnections = HubConnections.Users[userId];
            userConnections.Remove(Context.ConnectionId);
            HubConnections.Users.Remove(userId);
            if(userConnections.Any())
                HubConnections.Users.Add(userId,userConnections);
        }
        if(!string.IsNullOrEmpty(userId))
        {
            var userName = (await _context.Users.FirstOrDefaultAsync(u => u.Id == userId))!.UserName;
            await Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReveiveUserDisconnected",userId,userName);
            HubConnections.AddUserConnection(userId,Context.ConnectionId);
        }
    }

    public async Task SendAddRoomMessage(int maxRoom,int roomId,string roomName)
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = (await _context.Users.FirstOrDefaultAsync(u => u.Id == userId))!.UserName;

        await Clients.All.SendAsync("ReceiveAddRoomMessage",maxRoom,roomId,roomName,userId,userName);
    }

    public async Task SendDeleteRoomMessage(int deleted,int selected,string roomName)
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = (await _context.Users.FirstOrDefaultAsync(u => u.Id == userId))!.UserName;

        await Clients.All.SendAsync("ReceiveDeleteRoomMessage",deleted,selected,roomName,userName);
    }

    public async Task SendPublicMessage(int roomId,string message,string roomName)
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = (await _context.Users.FirstOrDefaultAsync(u => u.Id == userId))!.UserName;

        await Clients.All.SendAsync("ReceivePublicMessage",roomId,userId,userName,message,userName);
    }

    public async Task SendPrivateMessage(string receiverId,string message,string receiverName)
    {
        var senderId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var senderName = (await _context.Users.FirstOrDefaultAsync(u => u.Id == senderId))!.UserName;

        var users = new string[] { senderId,receiverId };

        await Clients.Users(users).SendAsync("ReceivePrivateMessage",senderId,senderName,receiverId,message,Guid.NewGuid(),receiverName);
    }
}
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class LikeHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
    }

    public async Task UpdateLikesCount(int postId, int likesCount)
    {
        await Clients.All.SendAsync("ReceiveLikesUpdate", postId, likesCount);
        Console.WriteLine($"Updated likes count: {likesCount}");
    }
}

using Microsoft.AspNetCore.SignalR;

public class MovieHub : Hub
{
    public async Task NotifyMovieChange()
    {
        await Clients.All.SendAsync("ReceiveMovieUpdate");
    }
}
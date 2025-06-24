using Microsoft.AspNetCore.SignalR;

namespace TestBlazor.Hubs
{
    public class MovieHub : Hub
    {
        public async Task NotifyMovieChange()
        {
            await Clients.All.SendAsync("ReceiveMovieUpdate");
        }
    }
}

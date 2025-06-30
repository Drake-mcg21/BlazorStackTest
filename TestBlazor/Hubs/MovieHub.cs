using Microsoft.AspNetCore.SignalR;
using TestBlazor.Models;

namespace TestBlazor.Hubs
{
    public class MovieHub : Hub<IMovieClient>
    {
        public async Task BroadcastMovieChange(Movie movie)
        {
            await Clients.All.MovieChanged(movie);
        }
    }
}

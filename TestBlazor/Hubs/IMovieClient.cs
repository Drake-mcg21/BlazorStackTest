using System.Threading.Tasks;
using TestBlazor.Models;

namespace TestBlazor.Hubs
{
    public interface IMovieClient
    {
        Task MovieChanged(Movie movie);
    }
}

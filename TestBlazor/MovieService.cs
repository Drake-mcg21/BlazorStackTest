// Services/MovieService.cs
using Dapper;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using System.Data.SqlClient;
using TestBlazor.Hubs;
using TestBlazor.Models;

public class MovieService
{
    private readonly string _connectionString;
    private readonly IHubContext<MovieHub> _hubContext;

    public MovieService(IConfiguration config, ILogger<MovieService> logger, IHubContext<MovieHub> hubContext)
    {
        _connectionString = config.GetConnectionString("MovieDb");
        _hubContext = hubContext;
    }

    public async Task<IEnumerable<Movie>> GetAllMovies()
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        return await db.QueryAsync<Movie>("SELECT * FROM Movies ORDER BY Title");
    }

    public async Task<Movie> GetMovieById(int id)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        return await db.QueryFirstOrDefaultAsync<Movie>(
            "SELECT * FROM Movies WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateMovie(Movie movie)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        const string sql = @"
            INSERT INTO Movies (Title, Genre, ReleaseDate, BoxOfficeSales)
            VALUES (@Title, @Genre, @ReleaseDate, @BoxOfficeSales);
            SELECT CAST(SCOPE_IDENTITY() as int)";

        var id = await db.ExecuteScalarAsync<int>(sql, movie);
        movie.Id = id;
        await _hubContext.Clients.All.MovieChanged(movie);
        return id;
    }

    public async Task<bool> UpdateMovie(Movie movie)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        const string sql = @"
            UPDATE Movies 
            SET Title = @Title, 
                Genre = @Genre, 
                ReleaseDate = @ReleaseDate, 
                BoxOfficeSales = @BoxOfficeSales
            WHERE Id = @Id";

        var affectedRows = await db.ExecuteAsync(sql, movie);
        if (affectedRows == 1)
        {
            await _hubContext.Clients.All.MovieChanged(movie);
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteMovie(int id)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        var affectedRows = await db.ExecuteAsync(
            "DELETE FROM Movies WHERE Id = @Id", new { Id = id });
        if (affectedRows == 1)
        {
            await _hubContext.Clients.All.MovieChanged(new Movie { Id = id });
            return true;
        }
        return false;
    }
}
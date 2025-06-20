// Services/MovieService.cs
using Dapper;
using System.Data;
using System.Data.SqlClient;

public class MovieService
{
    private readonly string _connectionString;

    public MovieService(IConfiguration config, ILogger<MovieService> logger)
    {
        _connectionString = config.GetConnectionString("MovieDb");
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

        return await db.ExecuteScalarAsync<int>(sql, movie);
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
        return affectedRows == 1;
    }

    public async Task<bool> DeleteMovie(int id)
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        var affectedRows = await db.ExecuteAsync(
            "DELETE FROM Movies WHERE Id = @Id", new { Id = id });
        return affectedRows == 1;
    }
}
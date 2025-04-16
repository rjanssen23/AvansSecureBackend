using Microsoft.Data.SqlClient;
using ProjectMap.WebApi.Repositories;
using Dapper;
using ProjectMap.WebApi.Models; // ← assuming your class is in this namespace

public class UserEnvironmentRepository : IEnvironmentRepository
{
    private readonly string _connectionString;

    public UserEnvironmentRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<UserEnvironment>> GetEnvironmentsByUserIdAsync(Guid userId)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = "SELECT * FROM Environment WHERE UserId = @UserId";
        return await connection.QueryAsync<UserEnvironment>(query, new { UserId = userId });
    }

    public async Task<UserEnvironment> InsertAsync(UserEnvironment environment)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = "INSERT INTO Environment (Id, Name, UserId) VALUES (@Id, @Name, @UserId)";
        await connection.ExecuteAsync(query, environment);
        return environment;
    }

    public async Task<UserEnvironment> ReadAsync(Guid environmentId)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = "SELECT * FROM Environment WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<UserEnvironment>(query, new { Id = environmentId });
    }

    public async Task UpdateAsync(UserEnvironment environment)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = "UPDATE Environment SET Name = @Name WHERE Id = @Id";
        await connection.ExecuteAsync(query, environment);
    }

    public async Task DeleteAsync(Guid environmentId)
    {
        using var connection = new SqlConnection(_connectionString);
        var query = "DELETE FROM Environment WHERE Id = @Id";
        await connection.ExecuteAsync(query, new { Id = environmentId });
    }
}





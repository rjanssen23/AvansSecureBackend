using Dapper;
using Microsoft.Data.SqlClient;
using ProjectMap.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMap.WebApi.Repositories
{
    public class EnvironmentRepository : IEnvironmentRepository
    {
        private readonly string _connectionString;

        public EnvironmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Models.Environment>> GetEnvironmentsByUserIdAsync(Guid userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Environment WHERE UserId = @UserId";
            return await connection.QueryAsync<Models.Environment>(query, new { UserId = userId });
        }

        public async Task<Models.Environment> InsertAsync(Models.Environment environment)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "INSERT INTO Environment (Id, Name, UserId) VALUES (@Id, @Name, @UserId)";
            await connection.ExecuteAsync(query, environment);
            return environment;
        }

        public async Task<Models.Environment> ReadAsync(Guid environmentId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Environment WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Models.Environment>(query, new { Id = environmentId });
        }

        public async Task UpdateAsync(Models.Environment environment)
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
}



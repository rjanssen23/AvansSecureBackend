using Dapper;
using Microsoft.Data.SqlClient;
using ProjectMap.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMap.WebApi.Repositories
{
    public class ProfielKeuzeRepository : IProfielKeuzeRepository
    {
        private readonly string _connectionString;

        public ProfielKeuzeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<ProfielKeuze>> GetProfielKeuzesByUserIdAsync(Guid userId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM ProfielKeuze WHERE UserId = @UserId";
            return await connection.QueryAsync<ProfielKeuze>(query, new { UserId = userId });
        }

        public async Task<ProfielKeuze> InsertAsync(ProfielKeuze profielKeuze)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "INSERT INTO ProfielKeuze (Id, Name, Arts, GeboorteDatum, Avatar, UserId) VALUES (@Id, @Name, @Arts, @GeboorteDatum, @Avatar, @UserId)";
            await connection.ExecuteAsync(query, profielKeuze);
            return profielKeuze;
        }

        public async Task<ProfielKeuze> ReadAsync(Guid profielKeuzeId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM ProfielKeuze WHERE Id = @Id";
            return await connection.QuerySingleOrDefaultAsync<ProfielKeuze>(query, new { Id = profielKeuzeId });
        }

        public async Task UpdateAsync(ProfielKeuze profielKeuze)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "UPDATE ProfielKeuze SET Name = @Name, Arts = @Arts, GeboorteDatum = @GeboorteDatum, Avatar = @Avatar WHERE Id = @Id";
            await connection.ExecuteAsync(query, profielKeuze);
        }

        public async Task DeleteAsync(Guid profielKeuzeId)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM ProfielKeuze WHERE Id = @Id";
            await connection.ExecuteAsync(query, new { Id = profielKeuzeId });
        }
    }
}



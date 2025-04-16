using ProjectMap.WebApi.Models;

namespace ProjectMap.WebApi.Repositories
{
    public interface IEnvironmentRepository
    {
        Task DeleteAsync(Guid environmentId);
        Task<IEnumerable<Models.Environment>> GetEnvironmentsByUserIdAsync(Guid userId);
        Task<Models.Environment> InsertAsync(Models.Environment environment);
        Task<Models.Environment> ReadAsync(Guid environmentId);
        Task UpdateAsync(Models.Environment environment);
    }
}
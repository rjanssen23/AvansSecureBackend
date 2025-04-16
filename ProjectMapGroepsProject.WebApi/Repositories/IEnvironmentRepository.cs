using ProjectMap.WebApi.Models;

namespace ProjectMap.WebApi.Repositories
{
    public interface IEnvironmentRepository
    {
        Task DeleteAsync(Guid environmentId);
        Task<IEnumerable<Models.UserEnvironment>> GetEnvironmentsByUserIdAsync(Guid userId);
        Task<Models.UserEnvironment> InsertAsync(Models.UserEnvironment environment);
        Task<Models.UserEnvironment> ReadAsync(Guid environmentId);
        Task UpdateAsync(Models.UserEnvironment environment);
    }
}
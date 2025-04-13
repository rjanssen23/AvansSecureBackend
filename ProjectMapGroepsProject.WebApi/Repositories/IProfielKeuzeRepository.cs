using ProjectMap.WebApi.Models;

namespace ProjectMap.WebApi.Repositories
{
    public interface IProfielKeuzeRepository
    {
        Task DeleteAsync(Guid profielKeuzeId);
        Task<IEnumerable<ProfielKeuze>> GetProfielKeuzesByUserIdAsync(Guid userId);
        Task<ProfielKeuze> InsertAsync(ProfielKeuze profielKeuze);
        Task<ProfielKeuze> ReadAsync(Guid profielKeuzeId);
        Task UpdateAsync(ProfielKeuze profielKeuze);
    }
}
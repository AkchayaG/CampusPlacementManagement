using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface ICollegeRepository
    {
        Task<IEnumerable<College>> GetAllAsync();
        Task<College> GetByIdAsync(int id);
        Task<IEnumerable<College>> GetPendingCollegesAsync();
        Task AddAsync(College college);
        Task UpdateAsync(College college);
    }
}
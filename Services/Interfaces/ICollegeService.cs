using CampusPlacementManagement.DTOs;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICollegeService
    {
        Task<CollegeResponseDto> RegisterCollegeAsync(CollegeRegisterDto dto);
        Task<IEnumerable<CollegeResponseDto>> GetAllCollegesAsync();
        Task<CollegeResponseDto> GetCollegeByIdAsync(int id);
        Task<IEnumerable<CollegeResponseDto>> GetPendingCollegesAsync();
        Task<bool> ApproveCollegeAsync(int id);
        Task<bool> RejectCollegeAsync(int id);
        Task<bool> SuspendCollegeAsync(int id);
    }
}
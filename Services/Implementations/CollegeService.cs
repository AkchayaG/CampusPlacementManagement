using CampusPlacementManagement.DTOs;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations
{
    public class CollegeService : ICollegeService
    {
        private readonly ICollegeRepository _collegeRepository;

        public CollegeService(ICollegeRepository collegeRepository)
        {
            _collegeRepository = collegeRepository;
        }

        public async Task<CollegeResponseDto> RegisterCollegeAsync(CollegeRegisterDto dto)
        {
            var college = new College
            {
                CollegeName = dto.CollegeName,
                CollegeCode = dto.CollegeCode,
                UniversityName = dto.UniversityName,
                OfficialEmail = dto.OfficialEmail,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Website = dto.Website,
                Status = CollegeStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _collegeRepository.AddAsync(college);
            return MapToDto(college);
        }

        public async Task<IEnumerable<CollegeResponseDto>> GetAllCollegesAsync()
        {
            var colleges = await _collegeRepository.GetAllAsync();
            return colleges.Select(MapToDto);
        }

        public async Task<CollegeResponseDto> GetCollegeByIdAsync(int id)
        {
            var college = await _collegeRepository.GetByIdAsync(id);
            if (college == null) throw new KeyNotFoundException("College not found.");
            return MapToDto(college);
        }

        public async Task<IEnumerable<CollegeResponseDto>> GetPendingCollegesAsync()
        {
            var colleges = await _collegeRepository.GetPendingCollegesAsync();
            return colleges.Select(MapToDto);
        }

        public async Task<bool> ApproveCollegeAsync(int id)
        {
            var college = await _collegeRepository.GetByIdAsync(id);
            if (college == null) return false;

            college.Status = CollegeStatus.Approved;
            await _collegeRepository.UpdateAsync(college);
            return true;
        }

        public async Task<bool> RejectCollegeAsync(int id)
        {
            var college = await _collegeRepository.GetByIdAsync(id);
            if (college == null) return false;

            college.Status = CollegeStatus.Rejected;
            await _collegeRepository.UpdateAsync(college);
            return true;
        }

        public async Task<bool> SuspendCollegeAsync(int id)
        {
            var college = await _collegeRepository.GetByIdAsync(id);
            if (college == null) return false;

            college.Status = CollegeStatus.Suspended;
            await _collegeRepository.UpdateAsync(college);
            return true;
        }

        private static CollegeResponseDto MapToDto(College c) => new()
        {
            CollegeId = c.CollegeId,
            CollegeName = c.CollegeName,
            CollegeCode = c.CollegeCode,
            UniversityName = c.UniversityName,
            OfficialEmail = c.OfficialEmail,
            PhoneNumber = c.PhoneNumber,
            Address = c.Address,
            Website = c.Website,
            Status = c.Status.ToString(),
            CreatedAt = c.CreatedAt
        };
    }
}
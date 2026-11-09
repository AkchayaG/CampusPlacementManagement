namespace CampusPlacementManagement.DTOs
{
    public class RegisterCompanyHrDto
    {
        public required string CompanyName { get; set; }
        public required string HrName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Industry { get; set; }
        public string? Website { get; set; }
    }
}
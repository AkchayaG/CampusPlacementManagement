namespace CampusPlacementManagement.DTOs
{
    public class RegisterPoDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int CollegeId { get; set; }
}
}
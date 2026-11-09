namespace CampusPlacementManagement.DTOs
{
    public class RegisterStudentDto
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int CollegeId { get; set; }
        public required string Department { get; set; }
        public double CGPA { get; set; }
    }
}
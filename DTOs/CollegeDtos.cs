namespace CampusPlacementManagement.DTOs
{
    public class CollegeRegisterDto
    {
        public string CollegeName { get; set; }
        public string CollegeCode { get; set; }
        public string UniversityName { get; set; }
        public string OfficialEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
    }

    public class CollegeResponseDto
    {
        public int CollegeId { get; set; }
        public string CollegeName { get; set; }
        public string CollegeCode { get; set; }
        public string UniversityName { get; set; }
        public string OfficialEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
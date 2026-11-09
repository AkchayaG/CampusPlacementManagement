namespace CampusPlacementManagement.DTOs
{
    public class ScheduleInterviewDto
    {
        public int JobId { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public DateTime InterviewDate { get; set; }
        public string LocationOrLink { get; set; } = string.Empty;
    }
}
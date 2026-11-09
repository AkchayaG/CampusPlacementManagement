using System.ComponentModel.DataAnnotations;


namespace CampusPlacementManagement.Models
{
    public class JobApplication
    {
        public int Id { get; set; }

        [Required]
        public int JobId { get; set; }
        public Job? Job { get; set; }

        [Required]
        public string StudentId { get; set; } = string.Empty;
        public ApplicationUser? Student { get; set; }

        // Application Status: 0 = Applied, 1 = Shortlisted, 2 = Rejected, 3 = Selected
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }

    public enum ApplicationStatus
    {
        Applied = 0,
        Shortlisted = 1,
        Rejected = 2,
        Selected = 3
    }
}
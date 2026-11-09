using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusPlacementManagement.Models
{
    public class InterviewSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; } = null!;

        [Required]
        public string StudentId { get; set; } = string.Empty;
        [ForeignKey("StudentId")]
        public ApplicationUser Student { get; set; } = null!;

        [Required]
        public DateTime InterviewDate { get; set; }

        public string LocationOrLink { get; set; } = string.Empty; // Zoom/Teams link or Room No

        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
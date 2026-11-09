using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public decimal CTC { get; set; }

        public double MinimumCGPA { get; set; }

        // Which HR posted this job
        [Required]
        public string HrId { get; set; } = string.Empty;
        public ApplicationUser? Hr { get; set; }

        // Job Approval Status: 0 = Pending, 1 = Approved, 2 = Rejected
        public JobStatus Status { get; set; } = JobStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum JobStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
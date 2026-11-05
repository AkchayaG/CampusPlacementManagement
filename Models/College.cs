using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.Models
{
    public enum CollegeStatus
    {
        Pending,
        Approved,
        Rejected,
        Suspended,
        Inactive
    }

    public class College
    {
        [Key]
        public int CollegeId { get; set; }

        [Required]
        [MaxLength(200)]
        public string CollegeName { get; set; }

        [Required]
        [MaxLength(50)]
        public string CollegeCode { get; set; }

        [MaxLength(200)]
        public string UniversityName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string OfficialEmail { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(255)]
        public string Website { get; set; }

        [Required]
        public CollegeStatus Status { get; set; } = CollegeStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
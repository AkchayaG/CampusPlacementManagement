using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs
{
    public class CreateJobDto
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        public decimal CTC { get; set; }

        public double MinimumCGPA { get; set; }
    }
}
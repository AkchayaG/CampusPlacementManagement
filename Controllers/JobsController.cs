using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs;
using CampusPlacementManagement.Models;
using System.Security.Claims;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. HR or SuperAdmin posts a job (Status: Pending by default)
        [HttpPost]
        [Authorize(Roles = "CompanyHR,SuperAdmin")]
        public async Task<IActionResult> PostJob([FromBody] CreateJobDto model)
        {
            var hrId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(hrId))
                return Unauthorized(new { message = "User ID not found in token" });

            var job = new Job
            {
                Title = model.Title,
                Description = model.Description,
                CTC = model.CTC,
                MinimumCGPA = model.MinimumCGPA,
                HrId = hrId,
                Status = JobStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Job posted successfully. Pending approval.", jobId = job.Id });
        }

        // 2. Placement Officer or SuperAdmin approves the job
        [HttpPut("{id}/approve")]
        [Authorize(Roles = "SuperAdmin,PlacementOfficer")]
        public async Task<IActionResult> ApproveJob(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound(new { message = "Job not found" });

            job.Status = JobStatus.Approved;
            job.UpdatedAt = DateTime.UtcNow;

            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Job approved successfully!" });
        }
    }
}
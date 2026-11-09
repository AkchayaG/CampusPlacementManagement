using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs;
using CampusPlacementManagement.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

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
        // 3. Student applies for an approved job
        [HttpPost("{id}/apply")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ApplyForJob(int id)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized(new { message = "User ID not found in token" });

            // Check if job exists and is approved
            var job = await _context.Jobs.FindAsync(id);
            if (job == null || job.Status != JobStatus.Approved)
                return NotFound(new { message = "Approved job not found" });

            // Check if student already applied
            // Check if student already applied
            var existingApplication = await _context.JobApplications
                .Where(a => a.JobId == id && a.StudentId == studentId)
                .FirstOrDefaultAsync();

            if (existingApplication != null)
                return BadRequest(new { message = "You have already applied for this job." });

            var application = new JobApplication
            {
                JobId = id,
                StudentId = studentId,
                Status = ApplicationStatus.Applied,
                AppliedAt = DateTime.UtcNow
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Successfully applied for the job!", applicationId = application.Id });
        }
        [HttpPut("applications/{applicationId}/status")]
        [Authorize(Roles = "CompanyHR,PlacementOfficer")]
        public async Task<IActionResult> UpdateApplicationStatus(int applicationId, [FromBody] UpdateApplicationStatusDto dto)
        {
            // 👇 Split the query to fix type inference error
            var application = await _context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.Id == applicationId)
                .FirstOrDefaultAsync();

            if (application == null)
            {
                return NotFound(new { message = "Job application not found." });
            }

            // Update status (Make sure 'Status' property matches your Enum/String type)
            if (Enum.TryParse<ApplicationStatus>(dto.Status, true, out var parsedStatus))
            {
                application.Status = parsedStatus;
            }
            else
            {
                return BadRequest(new { message = "Invalid status value. Allowed values: Pending, Shortlisted, Rejected, Selected" });
            }

            // Oruvela JobApplication model-la UpdatedAt illana intha line-ah remove pannidalam
            // application.UpdatedAt = DateTime.UtcNow; 

            _context.JobApplications.Update(application);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Application status updated to '{dto.Status}' successfully." });
        }
    }
}
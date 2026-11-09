using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs;
using CampusPlacementManagement.Models;
using System.Security.Claims;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/interviews")]
    [ApiController]
    public class InterviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InterviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. CompanyHR / PlacementOfficer schedules an interview for a shortlisted student
        [HttpPost("schedule")]
        [Authorize(Roles = "CompanyHR,PlacementOfficer")]
        public async Task<IActionResult> ScheduleInterview([FromBody] ScheduleInterviewDto dto)
        {
            var schedule = new InterviewSchedule
            {
                JobId = dto.JobId,
                StudentId = dto.StudentId,
                InterviewDate = dto.InterviewDate,
                LocationOrLink = dto.LocationOrLink,
                Status = "Scheduled"
            };

            _context.InterviewSchedules.Add(schedule);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Interview scheduled successfully!", scheduleId = schedule.Id });
        }

        // 2. Student views their scheduled interviews
        [HttpGet("my-interviews")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetStudentInterviews()
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var interviews = await _context.InterviewSchedules
                .Where(i => i.StudentId == studentId)
                .Include(i => i.Job)
                .Select(i => new
                {
                    i.Id,
                    JobTitle = i.Job.Title,
                    i.InterviewDate,
                    i.LocationOrLink,
                    i.Status
                })
                .ToListAsync();

            return Ok(interviews);
        }
    }
}
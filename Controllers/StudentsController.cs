using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "SuperAdmin,PlacementOfficer")]
        public async Task<IActionResult> ApproveStudent(string id)
        {
            var student = await _userManager.FindByIdAsync(id);
            if (student == null)
                return NotFound(new { message = "Student not found" });

            student.AccountStatus = AccountStatus.Approved;
            student.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(student);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Student approved successfully!" });
        }
    }
}
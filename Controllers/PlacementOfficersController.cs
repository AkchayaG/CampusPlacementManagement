using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/placement-officers")]
    [ApiController]
    public class PlacementOfficersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PlacementOfficersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ApprovePlacementOfficer(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Placement Officer not found" });

            // Status-ah Approved-ku update panrathu
            user.AccountStatus = AccountStatus.Approved;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Placement Officer approved successfully!" });
        }
    }
}
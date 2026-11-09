using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CompaniesController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "SuperAdmin,PlacementOfficer")]
        public async Task<IActionResult> ApproveCompanyHr(string id)
        {
            var hrUser = await _userManager.FindByIdAsync(id);
            if (hrUser == null)
                return NotFound(new { message = "Company HR not found" });

            hrUser.AccountStatus = AccountStatus.Approved;
            hrUser.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(hrUser);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Company HR approved successfully!" });
        }
    }
}
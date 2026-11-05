using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampusPlacementManagement.DTOs;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/colleges")]
    [ApiController]
    public class CollegesController : ControllerBase
    {
        private readonly ICollegeService _collegeService;

        public CollegesController(ICollegeService collegeService)
        {
            _collegeService = collegeService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CollegeRegisterDto dto)
        {
            var result = await _collegeService.RegisterCollegeAsync(dto);
            return StatusCode(201, result);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _collegeService.GetAllCollegesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin,PlacementOfficer")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _collegeService.GetCollegeByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _collegeService.GetPendingCollegesAsync();
            return Ok(result);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Approve(int id)
        {
            var success = await _collegeService.ApproveCollegeAsync(id);
            if (!success) return NotFound(new { message = "College not found" });
            return NoContent();
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Reject(int id)
        {
            var success = await _collegeService.RejectCollegeAsync(id);
            if (!success) return NotFound(new { message = "College not found" });
            return NoContent();
        }

        [HttpPut("{id}/suspend")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Suspend(int id)
        {
            var success = await _collegeService.SuspendCollegeAsync(id);
            if (!success) return NotFound(new { message = "College not found" });
            return NoContent();
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore; // Needed for AnyAsync / FindAsync
using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs;
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context; // 1. Added DbContext

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(new { message = "Invalid Email or Password" });

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "SuperSecretKeyWithAtLeast32BytesLength!"));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email ?? string.Empty,
                Role = userRoles.FirstOrDefault() ?? "User"
            });
        }

        [HttpPost("register-po")]
        public async Task<IActionResult> RegisterPlacementOfficer([FromBody] RegisterPoDto model)
        {
            // 1. Check if College exists
            var college = await _context.Colleges.FindAsync(model.CollegeId);
            if (college == null)
                return NotFound(new { message = "College not found" });

            // 2. Check if College is Approved (Assuming Status == 1 or Approved enum)
            // Uru vela ungaloda College model-la status 'Approved' Enum/Int-ah irunthal antha condition potukonga
            if (college.Status != CollegeStatus.Approved)
                return BadRequest(new { message = "College is not approved by SuperAdmin yet!" });

            // 3. Create User using UserManager
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                CollegeId = model.CollegeId // CollegeId link panrathu
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // 4. Assign Role "PlacementOfficer"
            await _userManager.AddToRoleAsync(user, "PlacementOfficer");

            return Ok(new { message = "Placement Officer registered successfully. Pending approval." });
        }
        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentDto model)
        {
            // 1. Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Email is already taken." });

            // 2. Create ApplicationUser object
            var studentUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                CollegeId = model.CollegeId,
                AccountStatus = AccountStatus.Pending, // Or Approved based on your flow
                CreatedAt = DateTime.UtcNow
            };

            // 3. Create user with password using UserManager
            var result = await _userManager.CreateAsync(studentUser, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // 4. Assign 'Student' Role
            await _userManager.AddToRoleAsync(studentUser, "Student");

            return Ok(new { message = "Student registered successfully. Pending approval." });
        }
        [HttpPost("register-hr")]
        public async Task<IActionResult> RegisterCompanyHr([FromBody] RegisterCompanyHrDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Email is already taken." });

            var hrUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.HrName, // Or save company name if you have a separate property/table
                AccountStatus = AccountStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(hrUser, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Assign 'CompanyHR' Role
            await _userManager.AddToRoleAsync(hrUser, "CompanyHR");

            return Ok(new { message = "Company HR registered successfully. Pending approval." });
        }
    }
}
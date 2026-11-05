using Microsoft.AspNetCore.Identity;
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndSuperAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "SuperAdmin", "PlacementOfficer", "CompanyHR", "Student" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            string adminEmail = "admin@placement.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Platform Super Admin",
                    AccountStatus = AccountStatus.Approved,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@12345");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "SuperAdmin");
                }
            }
        }
    }
}
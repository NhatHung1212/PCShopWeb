using Microsoft.AspNetCore.Identity;
using ShopWeb.Models;

namespace ShopWeb.Data;

public class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Seed roles
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("Customer"))
        {
            await roleManager.CreateAsync(new IdentityRole("Customer"));
        }

        // Seed admin user
        var existingAdmin = await userManager.FindByEmailAsync("admin@pcshop.com");
        if (existingAdmin == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@pcshop.com",
                Email = "admin@pcshop.com",
                FullName = "Admin User",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        else
        {
            // Nếu user đã tồn tại, đảm bảo có role Admin
            if (!await userManager.IsInRoleAsync(existingAdmin, "Admin"))
            {
                await userManager.AddToRoleAsync(existingAdmin, "Admin");
            }
        }

        // Không seed categories và products vì đã có từ Laravel database
        // Chỉ seed Identity roles và admin user
    }
}

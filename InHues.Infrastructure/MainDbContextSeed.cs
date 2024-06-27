using InHues.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InHues.Infrastructure.Persistence
{
    public static class MainDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,MainDbContext context)
        {
            var initialRoles = new List<IdentityRole> { 
                new IdentityRole("Administrator"),
                new IdentityRole("Customer"),
            };
            var insertRoleTasks = initialRoles.Select(role => AddRoles(roleManager, role));
            await Task.WhenAll(insertRoleTasks);

            var administrator = new ApplicationUser { UserName = "admin", Email = "admin@inhues.com",IsEnabled = true };
            
            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                var adminRole = initialRoles.First(role => role.Name == "Administrator");
                var userRequest = await userManager.CreateAsync(administrator, "Pmmd.03162019!");
                await userManager.AddToRolesAsync(administrator, new[] { adminRole.Name });
                var userResult = userRequest.ToApplicationResult();
            }
        }
        private static async Task<IdentityResult> AddRoles(RoleManager<IdentityRole> roleManager,IdentityRole role) {
            if (roleManager.Roles.All(r => r.Name != role.Name))
            {
                return await roleManager.CreateAsync(role);
            }
            return null;
        }
    }
}

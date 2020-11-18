using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Enums;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyBlog.Utilities
{
    public static class SeedHelper
    {
        public static async Task SeedDataAsync(UserManager<BlogUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedAdmin(userManager);
            await SeedModerator(userManager);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
        }

        private static async Task SeedAdmin(UserManager<BlogUser> userManager)
        {
            if (await userManager.FindByEmailAsync("jessica@hedenskog.com") == null)
            {
                var admin = new BlogUser()
                {
                    Email = "jessica@hedenskog.com",
                    UserName = "jessica@hedenskog.com",
                    FirstName = "Jessica",
                    LastName = "Hedenskog",
                    DisplayName = "JessIsDaBest",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Inuyasha12!#");
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }

        private static async Task SeedModerator(UserManager<BlogUser> userManager)
        {
            if (await userManager.FindByEmailAsync("mountains@disney.com") == null)
            {
                var moderator = new BlogUser()
                {
                    Email = "mountains@disney.com",
                    UserName = "mountains@disney.com",
                    FirstName = "Splash",
                    LastName = "Mountain",
                    DisplayName = "SplashDaMountain",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(moderator, "Inuyasha12!#");
                await userManager.AddToRoleAsync(moderator, Roles.Moderator.ToString());
            }
        }

    }

}
       
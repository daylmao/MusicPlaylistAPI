using Microsoft.AspNetCore.Identity;
using MusicPlaylistAPI.Core.Domain.Enum;
using MusicPlaylistAPI.Infraestructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Infraestructure.Identity.Seeds
{
    public class DefaultSuperAdminRole
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var role = new User();
            role.UserName = "Daylight";
            role.FirstName = "Dayron";
            role.LastName = "Bello";
            role.Email = "aspnetpruebas@gmail.com";
            role.EmailConfirmed = true;
            role.PhoneNumberConfirmed = true;

            if (userManager.Users.All(u => u.Id != role.Id))
            {
                var email = await userManager.FindByEmailAsync(role.Email);
                if (email == null)
                {
                    await userManager.CreateAsync(role, "Dayr0n!");
                    await userManager.AddToRoleAsync(role, Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(role, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(role, Roles.SuperAdmin.ToString());

                }
            }
        }
    }
}

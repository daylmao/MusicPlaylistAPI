using Microsoft.AspNetCore.Identity;
using MusicPlaylistAPI.Infraestructure.Identity.Entities;
using MusicPlaylistAPI.Core.Domain.Enum;


namespace MusicPlaylistAPI.Infraestructure.Identity.Seeds
{
    public static class DefaultBasicRole
    {
        // Este método crea un usuario predeterminado si no existe en la base de datos
        // y le asigna un rol básico ("Basic") para garantizar que haya un usuario inicial configurado.
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var role = new User
            {
                UserName = "Daylight",         
                FirstName = "Dayron",          
                LastName = "Bello",           
                Email = "aspnetpruebas@gmail.com",
                EmailConfirmed = true,         
                PhoneNumberConfirmed = true   
            };

            if (userManager.Users.All(u => u.Id != role.Id))
            {
                var user = await userManager.FindByEmailAsync(role.Email);

                // Si no se encuentra un usuario con ese correo, lo crea con una contraseña predeterminada.
                if (user == null)
                {
                    await userManager.CreateAsync(role, "Dayr0n!"); // Crea el usuario con una contraseña.
                    await userManager.AddToRoleAsync(role, Roles.Basic.ToString()); // Asigna el rol "Basic".
                }
            }
        }

    }
}

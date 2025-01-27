using Microsoft.AspNetCore.Identity;
using MusicPlaylistAPI.Core.Domain.Enum;
using MusicPlaylistAPI.Infraestructure.Identity.Entities;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Infraestructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        // El método se encarga de crear los roles "SuperAdmin", "Admin" y "Basic" si no existen en la base de datos.
        // Utiliza el RoleManager para crear los roles necesarios y para asignar permisos en la aplicación.
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {

            // Crea el rol "SuperAdmin"
            // Esto asegura que siempre haya un rol con el nivel más alto de acceso disponible.
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));

            // Crea el rol "Admin" 
            // Este rol es para los usuarios con privilegios administrativos.
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

            // Crea el rol "Basic" 
            // Este es el rol predeterminado para los usuarios comunes en la aplicación.
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }


    }
}

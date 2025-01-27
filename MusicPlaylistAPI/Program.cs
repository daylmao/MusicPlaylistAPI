using MusicPlaylistAPI.Infraestructure.Persistence;
using MusicPlaylistAPI.Infraestructure.Identity;
using MusicPlaylistAPI.Infraestructure.Shared;
using MusicPlaylistAPI.Core.Application;
using Microsoft.AspNetCore.Identity;
using MusicPlaylistAPI.Infraestructure.Identity.Entities;
using MusicPlaylistAPI.Infraestructure.Identity.Seeds;
using MusicPlaylistAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerExtension();
builder.Services.AddVersioning();

//DI

builder.Services.AddPersistenceMethod(config);
builder.Services.AddApplicationMethod();
builder.Services.AddEmail(config);
builder.Services.AddIdentityMethod(config);

var app = builder.Build();

// Crear un nuevo alcance para acceder a los servicios registrados en la aplicación.
// Esto es necesario para obtener servicios con ciclo de vida "scoped" de manera segura.
using (var scope = app.Services.CreateScope())
{
    // El proveedor de servicios (`ServiceProvider`) contiene todas las dependencias que la aplicación puede usar.
    // Aquí se obtiene el `ServiceProvider` del alcance creado, para acceder a servicios como UserManager o RoleManager.
    var services = scope.ServiceProvider;

    try
    {
        // Obtener el servicio UserManager<User>, que se utiliza para manejar usuarios.
        // Por ejemplo, crear, actualizar o eliminar usuarios.
        var userManager = services.GetRequiredService<UserManager<User>>();

        // Obtener el servicio RoleManager<IdentityRole>, que se utiliza para manejar roles.
        // Por ejemplo, crear o asignar roles como "Admin", "User", etc.
        var rolManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Inicializar roles básicos necesarios para la aplicación.
        // Por ejemplo, roles como "Usuario básico".
        await DefaultBasicRole.SeedAsync(userManager, rolManager);

        // Inicializar otros roles adicionales personalizados que la aplicación pueda necesitar.
        // Por ejemplo, roles específicos como "Moderador", "Editor", etc.
        await DefaultRoles.SeedAsync(userManager, rolManager);

        // Inicializar el rol de "Super Administrador", que suele tener todos los permisos posibles.
        // También podría crear un usuario por defecto que tenga este rol asignado.
        await DefaultSuperAdminRole.SeedAsync(userManager, rolManager);
    }
    catch (Exception)
    {

    }
   
}

    Console.WriteLine("Iniciando consulta...");
    var resultado = await ConsultarBaseDeDatos();
    Console.WriteLine($"Resultado: {resultado}");

    static async Task<string> ConsultarBaseDeDatos()
    {
        await Task.Delay(3000);
        return "Datos obtenidos de la base de datos";
    }


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

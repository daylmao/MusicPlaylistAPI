using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicPlaylistAPI.Infraestructure.Persistence.Context;


namespace MusicPlaylistAPI.Infraestructure.Persistence
{
    public static class AddPersistence
    {
        public static void AddPersistenceMethod(this IServiceCollection Service, IConfiguration Configuration )
        {
            #region Connection

            Service.AddDbContext<PlaylistContext>(b =>
            {
                b.UseSqlServer(Configuration.GetConnectionString("PlaylistConnection"),
                c => c.MigrationsAssembly(typeof(PlaylistContext).Assembly.FullName));
            });

            #endregion

        }
    }
}

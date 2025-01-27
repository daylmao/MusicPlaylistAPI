using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicPlaylistAPI.Infraestructure.Identity.Context;
using MusicPlaylistAPI.Infraestructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Infraestructure.Identity
{
    public static class AddIdentity
    {
        public static void AddIdentityMethod(this IServiceCollection service, IConfiguration configuration)
        {
            #region Connection
            service.AddDbContext<IdentityContext>(b =>
            {
                b.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                c => c.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
            });
            #endregion


            #region Identity
            service.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
            service.AddAuthentication();
            #endregion

        }
    }
}

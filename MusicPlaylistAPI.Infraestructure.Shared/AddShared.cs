using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using MusicPlaylistAPI.Core.Domain.Settings;
using MusicPlaylistAPI.Infraestructure.Shared.Services;

namespace MusicPlaylistAPI.Infraestructure.Shared
{
    public static class AddShared
    {
        public static void AddEmail(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}

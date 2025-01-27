using System.Collections;
using System.Runtime.CompilerServices;
using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace MusicPlaylistAPI.Extensions
{
    public static class ServiceExtension
    {
        public static void AddSwaggerExtension(this IServiceCollection service)
        {
            service.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MusicPlaylist API",
                    Description = "Just a simple project",
                    Contact = new OpenApiContact
                    {
                        Name = "Dayron Bello",
                        Email = "aspnetpruebas@gmail.com"
                    }
                });
            });

        }
        public static void AddVersioning(this IServiceCollection service)
        {
            service.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
        }
    }

}

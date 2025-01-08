using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using MusicPlaylistAPI.Core.Application.Mapper;
using MusicPlaylistAPI.Core.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application
{
    public static class AddApplication
    {
        public static void AddApplicationMethod(this IServiceCollection Service)
        {
            #region mapper
            Service.AddAutoMapper(typeof(MapperProfile));
            #endregion

            #region services
            Service.AddScoped<IPlaylistService, PlaylistService>();
            Service.AddScoped<ISongService, SongService>();

            #endregion
        }
    }
}

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using MusicPlaylistAPI.Core.Application.Mapper;
using MusicPlaylistAPI.Core.Application.Services;
using MusicPlaylistAPI.Core.Application.Validators;


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
            Service.AddScoped<IUserService, UserService>();
            #endregion

            #region validators
            Service.AddValidatorsFromAssemblyContaining<CreatePlaylistValidator>();
            Service.AddValidatorsFromAssemblyContaining<PlaylistUpdateValidator>();
            Service.AddValidatorsFromAssemblyContaining<SongInsertValidator>();
            Service.AddValidatorsFromAssemblyContaining<SongUpdateValidator>();
            Service.AddValidatorsFromAssemblyContaining<UserInsertValidator>();
            Service.AddValidatorsFromAssemblyContaining<UserUpdateValidator>();
            #endregion
        }
    }
}

using AutoMapper;
using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
using MusicPlaylistAPI.Core.Application.DTOs.Song;
using MusicPlaylistAPI.Core.Application.DTOs.User;
using MusicPlaylistAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {

            #region song
            CreateMap<Song, SongDTO>();
           
            CreateMap<SongInsertDTO, Song>()
                .ForMember(dest => dest.SongId, opt => opt.Ignore()) 
                .ForMember(dest => dest.Playlists, opt => opt.Ignore()); 
            
            CreateMap<SongUpdateDTO, Song>()
                .ForMember(dest => dest.SongId, opt => opt.Ignore()) 
                .ForMember(dest => dest.Playlists, opt => opt.Ignore());
            #endregion

            #region playlist
            CreateMap<Playlist, PlaylistDTO>()
                .ForMember(dest => dest.Songs, opt => opt.MapFrom(src => src.Songs.Select(s => s.SongId)));

            
            CreateMap<PlaylistInsertDTO, Playlist>()
                .ForMember(dest => dest.PlaylistId, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore())
                .ForMember(dest => dest.Songs, opt => opt.Ignore()) 
                .ForMember(dest => dest.User, opt => opt.Ignore());

            
            CreateMap<PlaylistUpdateDTO, Playlist>()
                .ForMember(dest => dest.PlaylistId, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore())
                .ForMember(dest => dest.Songs, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            
            CreateMap<Playlist, PlaylistUpdateSongsDTO>()
                .ForMember(dest => dest.Songs, opt => opt.MapFrom(src => src.Songs.Select(s => s.SongId).ToList()));
            #endregion

            #region user


            CreateMap<UserInsertDTO, User>()
                    .ForMember(dest => dest.CreateAt, opt => opt.Ignore())
                    .ForMember(dest => dest.Playlists, opt => opt.Ignore()); 

                
                CreateMap<UserUpdateDTO, User>()
                    .ForMember(dest => dest.UserId, opt => opt.Ignore()) 
                    .ForMember(dest => dest.Playlists, opt => opt.Ignore()); 

                
                CreateMap<User, UserDTO>()
                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.CreateAt));
            #endregion

        }

    }
}


using AutoMapper;
using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
using MusicPlaylistAPI.Core.Application.DTOs.Song;
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

            // Mapeo Song -> SongDTO
            CreateMap<Song, SongDTO>();

            // Mapeo SongInsertDTO -> Song
            CreateMap<SongInsertDTO, Song>()
                .ForMember(dest => dest.SongId, opt => opt.Ignore()) // Ignorar SongId 
                .ForMember(dest => dest.Playlists, opt => opt.Ignore()); // Ignorar Playlists

            // Mapeo SongUpdateDTO -> Song
            CreateMap<SongUpdateDTO, Song>()
                .ForMember(dest => dest.SongId, opt => opt.Ignore()) // No modificar SongId en la actualización
                .ForMember(dest => dest.Playlists, opt => opt.Ignore()); // Ignorar Playlists

            // Mapeo Playlist -> PlaylistDTO
            CreateMap<Playlist, PlaylistDTO>()
                .ForMember(dest => dest.Songs, opt => opt.MapFrom(src => src.Songs.Select(s => s.SongId)));

            // Mapeo PlaylistInsertDTO -> Playlist
            CreateMap<PlaylistInsertDTO, Playlist>()
                .ForMember(dest => dest.PlaylistId, opt => opt.Ignore()) // Ignorar PlaylistId (generado automáticamente)
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore()) // Ignorar CreateAt (asignado manualmente)
                .ForMember(dest => dest.Songs, opt => opt.Ignore()) // Ignorar Songs
                .ForMember(dest => dest.User, opt => opt.Ignore()); // Ignorar User

            // Mapeo PlaylistUpdateDTO -> Playlist
            CreateMap<PlaylistUpdateDTO, Playlist>()
                .ForMember(dest => dest.PlaylistId, opt => opt.Ignore()) // Ignorar PlaylistId para evitar inconsistencias
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore()) // Ignorar CreateAt
                .ForMember(dest => dest.Songs, opt => opt.Ignore()) // Ignorar Songs
                .ForMember(dest => dest.User, opt => opt.Ignore()); // Ignorar User

            // Mapeo Playlist -> PlaylistUpdateSongsDTO
            CreateMap<Playlist, PlaylistUpdateSongsDTO>()
                .ForMember(dest => dest.Songs, opt => opt.MapFrom(src => src.Songs.Select(s => s.SongId).ToList()));
        }
    }
}

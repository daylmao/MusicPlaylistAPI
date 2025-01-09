using AutoMapper;
using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
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
         
            CreateMap<PlaylistInsertDTO, Playlist>()
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore()); 
            CreateMap<PlaylistUpdateDTO, Playlist>();
            CreateMap<Playlist, PlaylistDTO>();


        }
    }
}

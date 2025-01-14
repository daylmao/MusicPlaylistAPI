using AutoMapper;
using MusicPlaylistAPI.Core.Application.DTOs.User;
using MusicPlaylistAPI.Core.Application.Interfaces.Repository;
using MusicPlaylistAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<UserDTO, UserInsertDTO, UserUpdateDTO>
    {
        
    }
}

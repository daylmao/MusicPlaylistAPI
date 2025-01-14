using AutoMapper;
using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
using MusicPlaylistAPI.Core.Application.DTOs.User;
using MusicPlaylistAPI.Core.Application.Interfaces.Repository;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using MusicPlaylistAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserDTO> GetByIdAsync(Guid Id)
        {
            var found = await _userRepository.GetByIdAsync(Id);
            if (found == null)
            {
                return null;
            }
            return _mapper.Map<UserDTO>(found);
        }
        public async Task<UserDTO> CreateAsync(UserInsertDTO Insert)
        {
            var newInfo = _mapper.Map<User>(Insert);
            if (newInfo == null) return null;

            newInfo.CreateAt = DateTime.UtcNow;
            await _userRepository.CreateAsync(newInfo);
            return _mapper.Map<UserDTO>(newInfo);
        }

        public async Task<UserDTO> UpdateAsync(Guid Id, UserUpdateDTO Update)
        {
            var found = await _userRepository.GetByIdAsync(Id);
            if (found == null) return null;

            var updateInfo = _mapper.Map(Update,found);
            await _userRepository.UpdateAsync(updateInfo);
            return _mapper.Map<UserDTO>(updateInfo);
        }
    }
}

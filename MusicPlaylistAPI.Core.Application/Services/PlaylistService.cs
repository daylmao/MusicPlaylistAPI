using AutoMapper;
using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
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
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMapper _mapper;        

        public PlaylistService(IPlaylistRepository playlistRepository, IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlaylistDTO>> GetAllAsync()
        {
            var getAll = await _playlistRepository.GetAllAsync();
            return getAll.Select(b => _mapper.Map<PlaylistDTO>(b));
        }

        public async Task<PlaylistDTO> GetByIdAsync(int Id)
        {
            var found = await _playlistRepository.GetByIdAsync(Id);
            if (found == null)
            {
                return null;
            }
            return _mapper.Map<PlaylistDTO>(found);
        }

        public async Task<PlaylistDTO> CreateAsync(PlaylistInsertDTO Insert)
        {
            var newInfo = _mapper.Map<Playlist>(Insert);
            if (newInfo == null)
            {
                return null;
            }
            newInfo.DateCreated = DateTime.Now;
            await _playlistRepository.InsertAsync(newInfo);
            return _mapper.Map<PlaylistDTO>(newInfo);
        }

        public async Task<PlaylistDTO> UpdateAsync(int Id, PlaylistUpdateDTO Update)
        {
            var oldData = await _playlistRepository.GetByIdAsync(Id);
            if (oldData == null)
            {
                return null;
            }
            var newInfo = _mapper.Map(Update, oldData);
            await _playlistRepository.UpdateAsync(newInfo);
            return _mapper.Map<PlaylistDTO>(newInfo);
        }

        public async Task<PlaylistDTO> DeleteAsync(int Id)
        {
            var found = await _playlistRepository.GetByIdAsync(Id);
            if (found == null)
            {
                return null;
            }

            await _playlistRepository.DeleteAsync(found);
            return _mapper.Map<PlaylistDTO>(found);
        }
    }
}

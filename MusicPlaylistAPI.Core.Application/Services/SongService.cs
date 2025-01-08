using AutoMapper;
using MusicPlaylistAPI.Core.Application.DTOs.Song;
using MusicPlaylistAPI.Core.Application.Interfaces.Repository;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using MusicPlaylistAPI.Core.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Services
{
    public class SongService : ISongService
    {
        private readonly IMapper _mapper;
        private readonly ISongRepository _songRepository;

        public async Task<IEnumerable<SongDTO>> GetAllAsync()
        {
            var getAll = await _songRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SongDTO>>(getAll);
        }

        public async Task<SongDTO> GetByIdAsync(int Id)
        {
            var found = await _songRepository.GetByIdAsync(Id);
            if (found == null)
            {
                return null;
            }
            return _mapper.Map<SongDTO>(found);
        }
        public async Task<SongDTO> CreateAsync(SongInsertDTO Insert)
        {
            var insert = _mapper.Map<Song>(Insert);
            if (insert == null)
            {
                return null;
            }
            await _songRepository.InsertAsync(insert);
            return _mapper.Map<SongDTO>(insert);
        }
        public async Task<IEnumerable<SongDTO>> GetByTitleAndArtist(string? title, string? artist)
        {
            var filtered = await _songRepository.GetByTitleAndArtist(title, artist);
            return _mapper.Map<IEnumerable<SongDTO>>(filtered);
        }

        public async Task<SongDTO> UpdateAsync(int Id, SongUpdateDTO Update)
        {
            var found = await _songRepository.GetByIdAsync(Id);
            if (found == null)
            {
                return null;
            }
            var newInfo = _mapper.Map(Update, found);
            return _mapper.Map<SongDTO>(newInfo);
        }
        public async Task<SongDTO> DeleteAsync(int Id)
        {
            var found = await _songRepository.GetByIdAsync(Id);
            if (found == null)
            {
                return null;
            }
            await _songRepository.DeleteAsync(found);
            return _mapper.Map<SongDTO>(found);
        }
    }
}

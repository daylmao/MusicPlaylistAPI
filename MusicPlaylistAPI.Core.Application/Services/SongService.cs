using AutoMapper;
using MusicPlaylistAPI.Core.Application.DTOs.Song;
using MusicPlaylistAPI.Core.Application.DTOs.User;
using MusicPlaylistAPI.Core.Application.Interfaces.Repository;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using MusicPlaylistAPI.Core.Domain.Entities;


namespace MusicPlaylistAPI.Core.Application.Services
{
    public class SongService : ISongService
    {
        private readonly IMapper _mapper;
        private readonly ISongRepository _songRepository;

        public SongService(IMapper mapper, ISongRepository songRepository)
        {
            _mapper = mapper;
            _songRepository = songRepository;
        }

        public async Task<IEnumerable<SongDTO>> GetAllAsync()
        {
            var getAll = await _songRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SongDTO>>(getAll);
        }

        public async Task<SongDTO> GetByIdAsync(Guid Id)
        {
            var found = await _songRepository.GetByIdAsync(Id);
            if (found == null)
            {
                return null;
            }
            return _mapper.Map<SongDTO>(found);
        }
        public async Task<IEnumerable<SongDTO>?> GetByIdsAsync(IEnumerable<Guid> songIds)
        {
            if (songIds == null || !songIds.Any())
            {
                return null;
            }

            var songs = await _songRepository.GetByIdsAsync(songIds);
            return songs.Select(song => _mapper.Map<SongDTO>(song));
        }

        public async Task<SongDTO> CreateAsync(SongInsertDTO Insert)
        {
            var insert = _mapper.Map<Song>(Insert);
            if (insert == null)
            {
                return null;
            }
            await _songRepository.CreateAsync(insert);
            return _mapper.Map<SongDTO>(insert);
        }

        public async Task<SongDTO> UpdateAsync(Guid Id, SongUpdateDTO Update)
        {
            var oldData = await _songRepository.GetByIdAsync(Id);
            if (oldData == null)
            {
                return null;
            }
            var newInfo = _mapper.Map(Update, oldData);
            await _songRepository.UpdateAsync(newInfo);
            return _mapper.Map<SongDTO>(newInfo);
        }

        public async Task<IEnumerable<SongDTO>> GetByTitleAndArtist(string? title, string? artist)
        {
            var filtered = await _songRepository.GetByTitleAndArtist(title, artist);
            if (filtered == null)
            {
                return null;
            }
            return _mapper.Map<IEnumerable<SongDTO>>(filtered);
        }

        public async Task<SongDTO> DeleteAsync(Guid Id)
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

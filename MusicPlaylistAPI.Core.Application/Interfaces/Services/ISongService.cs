using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
using MusicPlaylistAPI.Core.Application.DTOs.Song;
using MusicPlaylistAPI.Core.Application.DTOs.User;
using MusicPlaylistAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Interfaces.Services
{
    public interface ISongService: IGenericService<SongDTO,SongInsertDTO, SongUpdateDTO>
    {
        Task<IEnumerable<SongDTO>> GetAllAsync();
        Task<IEnumerable<SongDTO>> GetByTitleAndArtist(string? title, string? artist);
        Task<SongDTO> DeleteAsync(Guid Id);
    }
}

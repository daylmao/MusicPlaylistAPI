using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
using MusicPlaylistAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Interfaces.Services
{
    public interface IPlaylistService: IGenericService<PlaylistDTO,PlaylistInsertDTO,PlaylistUpdateDTO>
    {
        Task<IEnumerable<PlaylistDTO>> GetAllAsync();
        Task<PlaylistUpdateSongsDTO> UpdatePlaylistSongs(Guid playlistId, List<Guid> songIds);
        Task<PlaylistDTO> DeleteAsync(Guid Id);
    }
}

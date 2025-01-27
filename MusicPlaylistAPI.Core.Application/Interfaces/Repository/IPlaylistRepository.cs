using MusicPlaylistAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Interfaces.Repository
{
    public interface IPlaylistRepository: IGenericRepository<Playlist>
    {
        Task<IEnumerable<Playlist>> GetAllAsync();
        Task<Playlist> UpdatePlaylistSongs(Guid playlistId, List<Guid> songIds); 

    }
}
